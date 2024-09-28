using Core.ApiConfigurations;
using Core.Domain.ResponseResult;
using Front.MVC.Models;
using Front.MVC.Services.Checkout.Interfaces;
using Front.MVC.Services.Customer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Front.MVC.Controllers
{
    public class OrderController : MainControllerMvc
    {
        private readonly ICustomerService _customerService;
        private readonly ICheckoutService _checkoutService;

        public OrderController(ICustomerService customerService,
            ICheckoutService checkoutService)
        {
            _customerService = customerService; 
            _checkoutService = checkoutService;
        }

        [HttpGet]
        [Route("delivery-address")]
        public async Task<IActionResult> DeliveryAddress()
        {
            var cart = await _checkoutService.GetCart();
            
            if (cart.Items.Count == 0) 
                return RedirectToAction("Index", "ShoppingCart");

            var endereco = await _customerService.GetAddress();
            var order = _checkoutService.ToOrder(cart, endereco);

            return View(order);
        }

        [HttpGet]
        [Route("payment")]
        public async Task<IActionResult> Payment()
        {
            var cart = await _checkoutService.GetCart();
            if (cart.Items.Count == 0) return RedirectToAction("Index", "ShoppingCart");

            var address = await _customerService.GetAddress();

            var order = _checkoutService.ToOrder(cart, address);

            return View(order);
        }

        [HttpPost]
        [Route("finish-order")]
        public async Task<IActionResult> FinishOrder(TransactionViewModel transaction)
        {

            CartViewModel? cart = await _checkoutService.GetCart();
            AddressViewModel? address = await _customerService.GetAddress();

            if (!ModelState.IsValid) 
            {
                return View("Payment", _checkoutService.ToOrder(await _checkoutService.GetCart(), address));
            }

            var retorno = await _checkoutService.FinishOrder(transaction);

            if (HasErrors(retorno))
            {
                if (cart.Items.Count == 0) return RedirectToAction("Index", "ShoppingCart");
                 
                var orderMap = _checkoutService.ToOrder(cart, address);
                return View("Payment", orderMap);
            }

            return RedirectToAction("OrderDone");
        }

        [HttpGet]
        [Route("order-done")]
        public async Task<IActionResult> OrderDone()
        {
            return View("OrderDone", await _checkoutService.GetLastOrder());
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> MyOrders()
        {
            var model = await _checkoutService.GetByOrders();
            return View(model);
        }
    }
}
