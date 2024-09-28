using Core.ApiConfigurations;
using Core.Domain.ResponseResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Bff.Checkout.API.Services.gRPC.Interfaces;
using Store.Bff.Checkout.API.Services.Rest.Catalog.Interfaces;
using Store.Bff.Checkout.API.Services.Rest.Orders.Interfaces;
using Store.Bff.Checkout.API.Services.Rest.ShopCart.Interfaces;
using Store.Bff.Checkout.Models;

namespace Store.Bff.Checkout.Controllers.V1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class CheckoutController : MainControllerApi
    {
        private readonly ICatalogService _catalogService;
        private readonly IShopCartService _shopCartService;
        private readonly IOrderService _orderService;
        private readonly IShopCartGrpcService _shopCartGrpcService;

        public CheckoutController(IAspNetUser aspNetUser, ICatalogService catalogService, IShopCartService shopCartService, IOrderService orderService, IShopCartGrpcService shopCartGrpcService) : base(aspNetUser)
        {
            _catalogService = catalogService;
            _shopCartService = shopCartService;
            _orderService = orderService;
            _shopCartGrpcService = shopCartGrpcService;
        }

        [HttpGet("cart")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse(await _shopCartGrpcService.GetCart());
            //return CustomResponse(await _shopCartService.GetCart());
        }

        [HttpGet("cart/quantity")]
        public async Task<int> GetCartQuantity()
        {
            ResponseResult? resposta = new ResponseResult();

            CartDTO cart = await _shopCartGrpcService.GetCart();
            //CartDTO cart = await _shopCartService.GetCart();

            return (cart?.Items.Sum(x => x.Quantity) ?? 0);
        }

        [HttpPost("cart/items")]
        public async Task<IActionResult> AddItems(CartItemDTO model)
        {
            ProductDTO product = await _catalogService.GetById(model.IdProduct);
            
            ICollection<string>? productErrors = product?.IsValid(model);

            if (productErrors?.Count > 0)
            {
                return CustomResponse(productErrors);
            }

            model.Name = product.Name;
            model.Price = product.Price;
            model.Image = product.Image;

            ResponseResult? resposta = await _shopCartService.AddItem(model);

            return CustomResponse(resposta);
        }

        [HttpPut("cart/items/{idProduct:guid}")]
        public async Task<IActionResult> UpdateItem(Guid idProduct, CartItemDTO model)
        {
            ProductDTO product = await _catalogService.GetById(model.IdProduct);

            CartDTO cart = await _shopCartService.GetCart();

            ICollection<string>? productErrors = product?.IsValid(model);

            if (productErrors?.Count > 0)
            {
                return CustomResponse(productErrors);
            }

            model.Name = product.Name;
            model.Price = product.Price;
            model.Image = product.Image;

            ResponseResult? resposta = await _shopCartService.UpdateItem(idProduct, model);

            return CustomResponse(resposta);
        }

        [HttpDelete("cart/items/{idProduct:guid}")]
        public async Task<IActionResult> DeleteItem(Guid idProduct)
        {
            ProductDTO product = await _catalogService.GetById(idProduct);

            ICollection<string>? productErrors = product?.IsValid();

            if (productErrors?.Count > 0)
            {
                return CustomResponse(productErrors);
            }
            
            ResponseResult? resposta = await _shopCartService.RemoveItem(idProduct);

            return CustomResponse(resposta);
        }

        [HttpPost("cart/apply-voucher")]
        public async Task<IActionResult> ApplyVoucher(VoucherDTO request)
        {
            VoucherDTO? voucher = await _orderService.GetVoucherCodeAsync(request.Code);

            var result = await _shopCartService.ApplyVoucher(voucher);

            if (voucher == null)
            {
                AddError("voucher invalid or not found"); 
            }

            return CustomResponse(result);
        }

    }
}
