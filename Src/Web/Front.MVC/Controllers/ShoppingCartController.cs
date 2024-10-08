using Core.ApiConfigurations;
using Core.Domain.ResponseResult;
using Front.MVC.Models;
using Front.MVC.Services.Checkout.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Front.MVC.Controllers
{
    [Authorize, Route("shopping-cart")]
    public class ShoppingCartController : MainControllerMvc
    {
        private readonly ICheckoutService _checkoutService;

        public ShoppingCartController(ICheckoutService shopCartService)
        {
            _checkoutService = shopCartService;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var x = await _checkoutService.GetCart();
            return View(x);
        }


        [Route("cart-quantity")]
        public async Task<IActionResult> GetCartQuantity()
        {
            return View(await _checkoutService.GetCartItemsQuantity());
        }


        [HttpPost]
        [Route("add-item")]
        public async Task<IActionResult> AddItem(CartItemViewModel shoppingCartItem)
        {
            ResponseResult? resposta = await _checkoutService.AddItem(shoppingCartItem);

            if (HasErrors(resposta)) return View("Index", await _checkoutService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("update-item")]
        public async Task<IActionResult> UpdateItem(Guid idProduct, int quantity)
        {
            CartItemViewModel? item = new CartItemViewModel { IdProduct = idProduct, Quantity = quantity };
            ResponseResult? resposta = await _checkoutService.UpdateItem(idProduct, item);

            if (HasErrors(resposta)) return View("Index", await _checkoutService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("remove-item")]
        public async Task<IActionResult> RemoveItem(Guid idProduct)
        {
            var resposta = await _checkoutService.RemoveItem(idProduct);

            if (HasErrors(resposta)) return View("Index", await _checkoutService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("apply-voucher")]
        public async Task<IActionResult> ApplyVoucher(string voucherCode)
        {
            var resposta = await _checkoutService.ApplyVoucher(voucherCode);

            if (HasErrors(resposta)) return View("Index", await _checkoutService.GetCart());

            return RedirectToAction("Index");
        }
    }
}