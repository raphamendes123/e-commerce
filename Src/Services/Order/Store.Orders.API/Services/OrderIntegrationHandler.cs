
using Core.Domain.Repository.DomainObjects;
using Core.Message.Integration.Background;
using MessageBus;
using Store.Orders.Infra.Data.Repositorys;
using Store.Orders.Infra.Data.Repositorys.Interfaces;

namespace Store.Orders.API.Services
{
    public class OrderIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public OrderIntegrationHandler(IMessageBus messageBus, IServiceProvider serviceProvider)
        {
            _bus = messageBus;
            _serviceProvider = serviceProvider;
        }

        //LEITURA DOS EVENTOS 
        private async Task SetSubscribers()
        {
            await _bus.SubscribeAsync<OrderCanceledIntegrationEvent>("OrderCanceled", CanceledOrder);
            await _bus.SubscribeAsync<OrderPaidIntegrationEvent>("OrderPaid", FinishOrder);
        }

        private async Task CanceledOrder(OrderCanceledIntegrationEvent message)
        {
            try
            {
                using (IServiceScope? scope = _serviceProvider.CreateScope())
                {
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                    var order = await orderRepository.GetById(message.IdOrder);
                    order.Cancel();

                    orderRepository.Update(order);

                    if (!(await orderRepository.UnitOfWork.Commit()))
                    {
                        throw new DomainException($"Problems while trying to cancel order {message.IdOrder}");
                    }
                }

            }
            catch (Exception ex)
            {
                 
            }
            
        }

        private async Task FinishOrder(OrderPaidIntegrationEvent message)
        {
            using (IServiceScope? scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                var order = await orderRepository.GetById(message.IdOrder);
                order.Finish();

                orderRepository.Update(order);

                if (!(await orderRepository.UnitOfWork.Commit()))
                {
                    throw new DomainException($"Problems found trying to finish o order {message.IdOrder}");
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SetSubscribers();
        }
    }
}
