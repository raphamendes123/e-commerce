using Store.Bff.Checkout.Models;

namespace Store.Bff.Checkout.API.Services.Rest.Catalog.Interfaces
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductDTO>> GetAll();

        Task<ProductDTO> GetById(Guid id);

        Task<IEnumerable<ProductDTO>> GetProducts(IEnumerable<Guid> ids);

    }
}
