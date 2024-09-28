using Store.ShopCart.API.Domain.Data.Entitys;
using Store.ShopCart.API.Business.Interfaces;
using Core.ApiConfigurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.ShopCart.API.Domain.Data.Contexts;

namespace Store.ShopCart.API.Controllers.V1
{

    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class CartController : MainControllerApi
    {
        private readonly CartDbContext _context;
        private readonly ICartBusiness _cartBusiness;

        public CartController(IAspNetUser aspNetUser, CartDbContext context, ICartBusiness cartBusiness) : base(aspNetUser)
        {
            _context = context;
            _cartBusiness = cartBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            return CustomResponse(await _cartBusiness.GetCart());
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(Guid idProduct, CartItem item)
        {
            return CustomResponse(await _cartBusiness.AddItem(item));
        }

        [HttpPut("{idProduct:guid}")]
        public async Task<IActionResult> UpdateItem(Guid idProduct, CartItem item)
        {
            return CustomResponse(await _cartBusiness.UpdateItem(idProduct, item));
        }

        [HttpDelete("{idProduct:guid}")]
        public async Task<IActionResult> RemoveItem(Guid idProduct)
        {
            return CustomResponse(await _cartBusiness.RemoveItem(idProduct));
        }

        [HttpPost("apply-voucher")]
        public async Task<IActionResult> ApplyVoucher(Voucher voucher)
        {
            return CustomResponse(await _cartBusiness.ApplyVoucher(voucher));
        }
    }
}
