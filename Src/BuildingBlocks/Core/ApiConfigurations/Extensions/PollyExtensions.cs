using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace Core.ApiConfigurations
{
    public static class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> WaitRetry()
        {
            var retryWaitPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(new[]
            {
            TimeSpan.FromMicroseconds(600),
            TimeSpan.FromMicroseconds(600),
            TimeSpan.FromMicroseconds(600)
            },
            onRetry: (outcome, timeSpan, retryCount, context) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Tentando pela {retryCount} vez");
                Console.ForegroundColor = ConsoleColor.White;
            });
            return retryWaitPolicy;
        }
    }
}
