using Core; 
using MessageBus;
using Core.Message;
using Store.Catalog.API.Services;

namespace Configurations
{
    public static class MessageQueueConfiguration
    {
        public static WebApplicationBuilder AddMessageQueueConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddMessageBus(builder.Configuration.GetMessageQueueConnection("DefaultConnection"))
                .AddHostedService<CatalogIntegrationHandler>(); 
            
            return builder;
        }
         
    }
}
