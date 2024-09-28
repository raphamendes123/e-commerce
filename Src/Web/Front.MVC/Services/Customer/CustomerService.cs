using Configurations;
using Core.Domain.ResponseResult;
using Front.MVC.Extensions;
using Front.MVC.Models;
using Front.MVC.Services.Abstracts;
using Front.MVC.Services.Customer.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;

namespace Front.MVC.Services.Customer
{
    public class CustomerService : Service, ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CustomerUrl);
        }

        public async Task<AddressViewModel> GetAddress()
        {
            var response = await _httpClient.GetAsync("/api/v1/customers/address");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            HandleResponseErrors(response);

            return await response.DeserializerObjectResponse<AddressViewModel>();
        }

        public async Task<ResponseResult> AddAddress(AddressViewModel address)
        {
            var enderecoContent = GetContent(address);

            var response = await _httpClient.PostAsync("/api/v1/customers/address", enderecoContent);

            if (!HandleResponseErrors(response)) return await response.DeserializerObjectResponse<ResponseResult>();  

            return ReturnOK();
        }
        public async Task<ResponseResult> UpdateAddress(AddressViewModel address)
        {
            var enderecoContent = GetContent(address);

            var response = await _httpClient.PutAsync("/api/v1/customers/address", enderecoContent);

            if (!HandleResponseErrors(response)) return await response.DeserializerObjectResponse<ResponseResult>();

            return ReturnOK();
        }
    }
}
