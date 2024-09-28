
using Core.Domain.Repository.DomainObjects;
using Core.Message.Integration;
using Core.Message.Integration.Background;
using MessageBus;
using Store.Payment.API.Domain.Data.Entitys;
using Store.Payment.API.Domain.Enums;
using Store.Payment.API.Domain.Models;
using Store.Payment.API.Services.Interfaces;

namespace Store.Payment.API.Services
{
    public class PaymentIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public PaymentIntegrationHandler(IMessageBus messageBus, IServiceProvider serviceProvider)
        {
            _bus = messageBus;
            _serviceProvider = serviceProvider;
        }


        //RESPONDA O EVENTO ENVIADO OrderStartedIntegrationEvent com retorno ResponseMessage
        private async Task SetResponse()
        {
            await _bus.RespondAsync<OrderStartedIntegrationEvent, ResponseMessage>(AuthorizeTransaction);

        }  
        //LEITURA DOS EVENTOS 
        private async Task SetSubscribers()
        {
            await _bus.SubscribeAsync<OrderLoweredStockIntegrationEvent>("OrderLowered", OrderReadyToCapturePaymentTransaction);
            await _bus.SubscribeAsync<OrderCanceledIntegrationEvent>("OrderCanceled", CanceledTransaction);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SetSubscribers();
            await SetResponse();
        }

      

        private async Task CanceledTransaction(OrderCanceledIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();

            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

            var Response = await pagamentoService.CancelTransaction(message.IdOrder);

            if (!Response.ValidationResult.IsValid)
                throw new DomainException($"Failed to cancel order payment {message.IdOrder}");
             
        }

        private async Task OrderReadyToCapturePaymentTransaction(OrderLoweredStockIntegrationEvent message)
        {
            
            using var scope = _serviceProvider.CreateScope();

            var pagamentoService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

            var Response = await pagamentoService.GetTransaction(message.IdOrder);

            if (!Response.ValidationResult.IsValid)
                throw new DomainException($"Error trying to get order payment {message.IdOrder}");


            //ENVIA EVENTO INFORMANDO QUE A ORDER FOI PAGA
            //VAI SER TRATADA NA API DE PEDIDOS E ATUALIZAR O STATUS DO PEDIDO PARA PAGO
            await _bus.PublishAsync(new OrderPaidIntegrationEvent(message.IdCustomer, message.IdOrder));

        }

        private async Task<ResponseMessage> AuthorizeTransaction(OrderStartedIntegrationEvent message)
        {

            using var scope = _serviceProvider.CreateScope();
            var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
            var transaction = new PaymentEntity
            {
                IdOrder = message.IdOrder,
                TypePayment = (EnumTypePayment)message.TypePayment,
                Amount = message.Amount,
                CreditCard = new CreditCard(
                                message.Holder, 
                                message.CardNumber, 
                                message.ExpirationDate, 
                                message.SecurityCode)
            };

            return await paymentService.AuthorizePayment(transaction);
        }
    }
}
