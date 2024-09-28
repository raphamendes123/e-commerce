
using Core.Message.Integration.Background;
using MessageBus;
using Store.Orders.API.Application.Queries.Interface;

namespace Store.Orders.API.Services
{
    public class OrderOrchestratorIntegrationHandler : IHostedService, IDisposable
    {
        private readonly ILogger<OrderOrchestratorIntegrationHandler> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public OrderOrchestratorIntegrationHandler(ILogger<OrderOrchestratorIntegrationHandler> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service started.");
            
            _timer = new Timer(OrderProcess, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service stop.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;

        }

        private async void OrderProcess(object state)
        {
            _logger.LogInformation("OrderProcess started.");

            using var scope = _serviceProvider.CreateScope();

            var orderQueries = scope.ServiceProvider.GetRequiredService<IOrderQuerie>();
            
            //RETORNA PEDIDO AUTORIZADO PARA ENVIAR EVENTO PARA QUEM QUISER USAR.
            var order = await orderQueries.GetAuthorizedOrders();

            if (order == null) 
                return;

            var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

            var authorizedOrder = 
                new OrderAuthorizedIntegrationEvent(
                    order.IdCustomer, 
                    order.Id,
                    order.OrderItems
                        .ToDictionary(p => p.IdProduct, p => p.Quantity));

            await bus.PublishAsync(authorizedOrder);

            _logger.LogInformation($"Order: {order.Id} foi enviado para baixa do estoque etc..");
        }
                
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
