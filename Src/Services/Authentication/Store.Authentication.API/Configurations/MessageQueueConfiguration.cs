using Core;
using Core.Message;
using MessageBus;

namespace Configurations
{
    public static class MessageQueueConfiguration
    {
        public static WebApplicationBuilder AddMessageQueueConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddMessageBus(builder.Configuration.GetMessageQueueConnection("DefaultConnection"));
            
            return builder;
        }
         
    }
}
