using Core.ApiConfigurations;
using Front.MVC.Services.Catalog;
using Microsoft.AspNetCore.Mvc; 

namespace Front.MVC.Controllers
{
    public class CatalogController : MainControllerMvc
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            var products = await _catalogService.GetAll(ps, page, q);
            ViewBag.Search = q;
            products.ReferenceAction = "Index";

            return View(products);
        }

        [HttpGet]
        [Route("Catalog/ProductDetails/{id}")]
        public async Task<IActionResult> ProductDetail(Guid id)
        {
            var product = await _catalogService.GetById(id);

            return View(product);
        }
    }
}
