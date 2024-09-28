using MediatR;

namespace Store.Customer.API.Application.Events
{
    public class RegisterCustomerEventHandler : INotificationHandler<RegisterCustomerEvent>
    {
        public Task Handle(RegisterCustomerEvent notification, CancellationToken cancellationToken)
        {
            // Enviar um evento de confirmacao
            return Task.CompletedTask;
        }
    }
}
