using Core.Mediator;
using Core.Message.Integration;
using Core.Message.Integration.Background;
using EasyNetQ;
using FluentValidation.Results;
using MessageBus;

namespace Store.Customer.API.Application.Commands.RegisterStore.Customer.Integration
{
    public class RegisterCustomerIntegrationHandler : BackgroundService
    {
        private IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RegisterCustomerIntegrationHandler(IServiceProvider serviceProvider, IMessageBus messageBus)
        {
            _serviceProvider = serviceProvider;
            _bus = messageBus;
        }

        public void SetResponder()
        {
            _bus.RespondAsync<RegisterCustomerIntegrationEvent, ResponseMessage>(async request => await RegisterCustomer(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            return Task.CompletedTask;
        }

        private void OnConnect(object? sender, ConnectedEventArgs e)
        {
            SetResponder();
        }

        private async Task<ResponseMessage> RegisterCustomer(RegisterCustomerIntegrationEvent request)
        {
            ValidationResult success;
            RegisterCustomerCommand? registerCustomerCommand =
                new RegisterCustomerCommand(id: request.Id, name: request.Name, email: request.Email, cpf: request.Cpf);

            //Pratica para resolver objetos, fora do padrao life cycle - devido singleton
            using (IServiceScope? scope = _serviceProvider.CreateScope())
            {
                IMediatorHandler? mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

                success = await mediator.SendCommand(registerCustomerCommand);
            }

            return new ResponseMessage(success);
        }
    }
}
