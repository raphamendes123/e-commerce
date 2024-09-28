using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Store.Catalog.API.Domain.Data.Entitys;
using Core.ApiConfigurations;
using Store.Catalog.API.Domain.Data.Repositorys.Interfaces;
using Store.Catalog.API.Domain.Models;

namespace Store.Catalog.API.Controllers.V1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/Products")]
    public class CatalogController : MainControllerApi
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository, IAspNetUser user) : base(user)
        {
            _productRepository = productRepository;
        }

        /*[HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductEntity>> GetProducts()
        {
            return await _productRepository.GetAllAsync();
        }*/

        [HttpGet]
        [AllowAnonymous]
        public async Task<PagedResult<ProductEntity>> Index([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            return await _productRepository.GetAllAsync(ps, page, q);
        }

        //[ClaimsAuthorize("Catalog", "GetProduct")]
        [HttpGet("{id}")]
        public async Task<ProductEntity> GetProduct(Guid id)
        {
            return await _productRepository.GetIdAsync(id);
        }

        [HttpGet("list/{ids}")]
        public async Task<IEnumerable<ProductEntity>> GetProductsIds(string ids)
        {
            return await _productRepository.GetProductsIds(ids);
        }

    }
}
