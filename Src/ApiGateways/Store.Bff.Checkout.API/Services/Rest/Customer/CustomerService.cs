using Configurations;
using Microsoft.Extensions.Options;
using Store.Bff.Checkout.API.Models;
using Store.Bff.Checkout.API.Services.Rest.Customer.Interfaces;
using Store.Bff.Checkout.Extensions;
using Store.Bff.Checkout.Services;
using System.Net;

namespace Store.Bff.Checkout.API.Services.Rest.Customer
{
    public class CustomerService : Service, ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CustomerUrl);
        }

        public async Task<AddressDTO> GetAddress()
        {
            var response = await _httpClient.GetAsync("/api/v1/customers/address");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            ResponseErrors(response);

            return await response.DeserializerResponse<AddressDTO>();
        }
    }
}
