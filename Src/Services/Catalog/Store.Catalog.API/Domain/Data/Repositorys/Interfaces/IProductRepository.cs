using Store.Catalog.API.Domain.Data.Entitys;
using Core.Domain.Repository.Data;
using Store.Catalog.API.Domain.Models;

namespace Store.Catalog.API.Domain.Data.Repositorys.Interfaces
{
    public interface IProductRepository : IRepository<ProductEntity>
    {
        Task<PagedResult<ProductEntity>> GetAllAsync(int pageSize, int pageIndex, string query = null);

        Task<IEnumerable<ProductEntity>> GetAllAsync();

        Task<ProductEntity> GetIdAsync(Guid id);

        Task<List<ProductEntity>> GetProductsIds(string ids);

        void Update(ProductEntity product);
    }
}
