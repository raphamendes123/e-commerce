using Core; 
using MessageBus;
using Core.Message;
using Store.Payment.API.Services;

namespace Configurations
{
    public static class MessageQueueConfiguration
    {
        public static WebApplicationBuilder AddMessageQueueConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddMessageBus(builder.Configuration.GetMessageQueueConnection("DefaultConnection"))
               .AddHostedService<PaymentIntegrationHandler>(); 
            
            return builder;
        }
         
    }
}
