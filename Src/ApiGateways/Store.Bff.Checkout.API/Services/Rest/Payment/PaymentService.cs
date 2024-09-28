using Core.Domain.ResponseResult;
using Microsoft.Extensions.Options;
using Configurations;
using Store.Bff.Checkout.Services;
using Store.Bff.Checkout.API.Services.Rest.Payment.Interfaces;

namespace Store.Bff.Checkout.API.Services.Rest.Payment
{
    public class PaymentService : Service, IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.PaymentUrl);
            _httpClient = httpClient;
        }


    }
}
