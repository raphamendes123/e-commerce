using Microsoft.Extensions.Options;
using Configurations;
using Store.Bff.Checkout.Models;
using Store.Bff.Checkout.Extensions;
using Store.Bff.Checkout.Services;
using Store.Bff.Checkout.API.Services.Rest.Catalog.Interfaces;

namespace Store.Bff.Checkout.API.Services.Rest.Catalog
{
    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            HttpResponseMessage? response = await _httpClient.GetAsync("api/v1/Catalog/products");

            ResponseErrors(response);

            return await response.DeserializerResponse<IEnumerable<ProductDTO>>();
        }

        public async Task<ProductDTO> GetById(Guid id)
        {
            HttpResponseMessage? response = await _httpClient.GetAsync($"api/v1/Catalog/products/{id}");

            ResponseErrors(response);

            return await response.DeserializerResponse<ProductDTO>();
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts(IEnumerable<Guid> ids)
        {
            var joinIds = string.Join(",", ids);

            HttpResponseMessage? response = await _httpClient.GetAsync($"api/v1/catalog/products/list/{joinIds}/");

            ResponseErrors(response);

            return await response.DeserializerResponse<IEnumerable<ProductDTO>>();
        }
    }
}
