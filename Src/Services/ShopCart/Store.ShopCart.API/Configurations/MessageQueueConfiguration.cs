using Core; 
using MessageBus;
using Core.Message;
using Core.Message.Integration.Background;
using Store.ShopCart.API.Services;

namespace Configurations
{
    public static class MessageQueueConfiguration
    {
        public static WebApplicationBuilder AddMessageQueueConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddMessageBus(builder.Configuration.GetMessageQueueConnection("DefaultConnection"))
                .AddHostedService<ShopCartIntegrationHandler>(); ;
            
            return builder;
        }
         
    }
}
