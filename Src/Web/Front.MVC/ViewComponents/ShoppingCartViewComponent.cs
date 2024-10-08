using Front.MVC.Models;
using Front.MVC.Services.Checkout.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Front.MVC.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly ICheckoutService _shopCartService;

        public ShoppingCartViewComponent(ICheckoutService shopCartService)
        {
            _shopCartService = shopCartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _shopCartService.GetCartItemsQuantity());
        }
    }
}