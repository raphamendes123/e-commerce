using Microsoft.Extensions.Configuration;

namespace Core.Message
{
    public static class ConfigurantionExtensions
    {
        public static string? GetMessageQueueConnection(this IConfiguration configuration, string name)
        {
            return configuration?.GetSection("ConnectionMessageQueue")[name];
        }
    }
}
