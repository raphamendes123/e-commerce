using Core;
using Store.Customer.API.Application.Commands.RegisterStore.Customer.Integration;
using MessageBus;
using Core.Message;

namespace Configurations
{
    public static class MessageQueueConfiguration
    {
        public static WebApplicationBuilder AddMessageQueueConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddMessageBus(builder.Configuration.GetMessageQueueConnection("DefaultConnection"))
                .AddHostedService<RegisterCustomerIntegrationHandler>(); ;
            
            return builder;
        }
         
    }
}
