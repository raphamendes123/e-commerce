using Microsoft.Extensions.Options;
using Configurations;
using Front.MVC.Extensions;
using Front.MVC.Models;
using Front.MVC.Services.Abstracts;

namespace Front.MVC.Services.Catalog
{
    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);
            _httpClient = httpClient;
        }

        public async Task<PagedViewModel<ProductViewModel>> GetAll(int pageSize, int pageIndex, string query = null)
        {
            HttpResponseMessage? response = await _httpClient.GetAsync($"api/v1/Catalog/products?ps={pageSize}&page={pageIndex}&q={query}"); ;

            HandleResponseErrors(response);

            return await response.DeserializerObjectResponse<PagedViewModel<ProductViewModel>>();
        }

        public async Task<ProductViewModel> GetById(Guid id)
        {
            HttpResponseMessage? response = await _httpClient.GetAsync($"api/v1/Catalog/products/{id}");

            HandleResponseErrors(response);

            return await response.DeserializerObjectResponse<ProductViewModel>();
        }
    }
}
