using Core.Message.Integration.Background;
using MediatR;
using MessageBus;

namespace Store.Orders.API.Application.Events
{
    //LANCAR O EVENTO PARA QUEM FOR UTILIZAR
    public class OrderEventHandler : INotificationHandler<OrderAuthorizedEvent>
    {
        private readonly IMessageBus _bus;

        public OrderEventHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(OrderAuthorizedEvent notification, CancellationToken cancellationToken)
        {
            await _bus.PublishAsync(new OrderAuthorizedIntegrationEvent(notification.IdCustomer, notification.IdOrder));
        }
    }
}
