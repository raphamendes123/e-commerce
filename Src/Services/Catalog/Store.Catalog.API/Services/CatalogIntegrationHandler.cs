
using Core.Domain.Repository.DomainObjects;
using Core.Message.Integration.Background;
using MessageBus;
using Store.Catalog.API.Domain.Data.Entitys;
using Store.Catalog.API.Domain.Data.Repositorys.Interfaces;

namespace Store.Catalog.API.Services
{
    public class CatalogIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CatalogIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }

        private async Task SetSubscribers()
        {
            await _bus.SubscribeAsync<OrderAuthorizedIntegrationEvent>("OrderAuthorized", SubtractStock);
        }

        private async Task SubtractStock(OrderAuthorizedIntegrationEvent message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var productsWithAvailableStock = new List<ProductEntity>();
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                var productsId = string.Join(",", message.Items.Select(c => c.Key));
                var products = await productRepository.GetProductsIds(productsId);


                //VALIDANDO A QUANTIDADE DOS PRODUTOS ENVIADO PELO EVENTO E QUANTIDADE DE PRODUTOS DO PEDIDO
                //ENVIA UM EVENTO INFORMANDO
                if (products.Count != message.Items.Count)
                {
                    await CancelOrderWithoutStock(message);
                    return;
                }

                foreach (var product in products)
                {
                    var productUnits = message.Items.FirstOrDefault(p => p.Key == product.Id).Value;

                    //PRODUTOS DIPONIVEIS E ATIVO
                    if (product.IsAvailable(productUnits))
                    {
                        //REMOVE DO ESTOQUE
                        product.TakeFromInventory(productUnits);

                        productsWithAvailableStock.Add(product);
                    }
                }

                //ENVIA EVENTO DE CANCELAR A ORDER, DEVIDO A QUANTIDADE DE ESTOQUE SEJA INFERIOR.
                if (productsWithAvailableStock.Count != message.Items.Count)
                {
                    await CancelOrderWithoutStock(message);
                    return;
                }

                foreach (var product in productsWithAvailableStock)
                {
                    productRepository.Update(product);
                }

                if (!await productRepository.UnitOfWork.Commit())
                {
                    //ANALISAR, SERIA BOM COLOCAR NUM DASHBOARD, DEVIDO FICAR NA LISTA DE ERROS, 
                    //E VAO SER EXECUTADOS NOVAMENTE.
                    throw new DomainException($"Problems updating stock for order {message.IdOrder}");
                }

                //ENVIO EVENTO PEDIDO BAIXADO 
                var productTaken = new OrderLoweredStockIntegrationEvent(message.IdCustomer, message.IdOrder);
                
                //PUBLICA PARA ALGUEM PEGAR >> API PAGAMENTO PARA ALTERAR O STATUS PARA PAGO
                await _bus.PublishAsync(productTaken);
            }
        }
        public async Task CancelOrderWithoutStock(OrderAuthorizedIntegrationEvent message)
        {
            var orderCancelled = new OrderCanceledIntegrationEvent(message.IdCustomer, message.IdOrder);
            await _bus.PublishAsync(orderCancelled);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SetSubscribers(); 
        }
    }
}
