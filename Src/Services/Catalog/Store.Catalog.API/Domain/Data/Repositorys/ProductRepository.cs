using Microsoft.EntityFrameworkCore;
using Store.Catalog.API.Domain.Data.Entitys;
using Core.Domain.Repository.Data;
using Store.Catalog.API.Domain.Data.Repositorys.Interfaces;
using Store.Catalog.API.Domain.Data.Contexts;
using Store.Catalog.API.Domain.Models;

namespace Store.Catalog.API.Domain.Data.Repositorys
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public ProductRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<PagedResult<ProductEntity>> GetAllAsync(int pageSize, int pageIndex, string query = null)
        {
            IQueryable<ProductEntity>? catalogQuery = _context.Products.AsQueryable();

            List<ProductEntity>? catalog = await catalogQuery.AsNoTrackingWithIdentityResolution()
                                            .Where(x => EF.Functions.Like(x.Name, $"%{query}%"))
                                            .OrderBy(x => x.Name)
                                            .Skip(pageSize * (pageIndex - 1))
                                            .Take(pageSize).ToListAsync();

            int total = await catalogQuery.AsNoTrackingWithIdentityResolution()
                                          .Where(x => EF.Functions.Like(x.Name, $"%{query}%"))
                                          .CountAsync();


            return new PagedResult<ProductEntity>()
            {
                List = catalog,
                TotalResults = total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query
            }; 
        }

        public async Task<ProductEntity> GetIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public void Add(ProductEntity model)
        {
            _context.Products.Add(model);
        }

        public void Update(ProductEntity model)
        {
            _context.Products.Update(model);
        }

        public async Task<List<ProductEntity>> GetProductsIds(string ids)
        {
            IEnumerable<(bool Ok, Guid Value)>? idsGuid = ids.Split(',')
                .Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

            if (!idsGuid.All(nid => nid.Ok)) 
                return new List<ProductEntity>();

            IEnumerable<Guid>? idsValue = idsGuid.Select(id => id.Value);

            return await _context.Products.AsNoTracking()
                .Where(p => idsValue.Contains(p.Id) && p.Active).ToListAsync();
        }


        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
