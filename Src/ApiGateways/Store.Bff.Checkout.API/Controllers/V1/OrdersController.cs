using Core.ApiConfigurations;
using Core.Domain.ResponseResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Bff.Checkout.API.Models;
using Store.Bff.Checkout.API.Services.Rest.Catalog.Interfaces;
using Store.Bff.Checkout.API.Services.Rest.Customer.Interfaces;
using Store.Bff.Checkout.API.Services.Rest.Orders.Interfaces;
using Store.Bff.Checkout.API.Services.Rest.ShopCart.Interfaces;
using Store.Bff.Checkout.Models;

namespace Store.Bff.Checkout.API.Controllers.V1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Checkout/[controller]")]
    public class OrdersController : MainControllerApi
    {
        private readonly ICatalogService _catalogService;
        private readonly IShopCartService _shopCartService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrdersController(IAspNetUser aspNetUser, ICatalogService catalogService, IShopCartService shopCartService, IOrderService orderService, ICustomerService customerService) : base(aspNetUser)
        {
            _catalogService = catalogService;
            _shopCartService = shopCartService;
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> RegisterOrder(OrderDTO order)
        {
            CartDTO? cart = await _shopCartService.GetCart();
            IEnumerable<ProductDTO>? products = await _catalogService.GetProducts(cart.Items.Select(p => p.IdProduct));
            AddressDTO? address = await _customerService.GetAddress();

            if (!await ValidateCartProducts(cart, products)) 
                return CustomResponse();

            order.AssignOrderDTO(cart, address);

            return CustomResponse(await _orderService.FinishOrder(order));
        }

        [HttpGet("last")]
        public async Task<IActionResult> LastOrder()
        {
            var order = await _orderService.GetLastOrder();
            if (order is null)
            {
                AddError("Order not found!");
                return CustomResponse();
            }

            return CustomResponse(order);
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> MyOrders()
        {
            IEnumerable<OrderDTO>? orders = await _orderService.GetOrders();

            return orders is null ? NotFound() : CustomResponse(orders);
        }

        private async Task<bool> ValidateCartProducts(CartDTO shoppingCart, IEnumerable<ProductDTO> products)
        {
            if (shoppingCart.Items.Count != products.Count())
            {
                var itensIndisponiveis = shoppingCart.Items.Select(c => c.IdProduct).Except(products.Select(p => p.Id)).ToList();

                foreach (var itemId in itensIndisponiveis)
                {
                    var cartItem = shoppingCart.Items.FirstOrDefault(c => c.IdProduct == itemId);
                    AddError($"The item {cartItem.Name} is not available at our catalog. Remove it from shoppingCart to continue shopping.");
                }

                return false;
            }

            foreach (CartItemDTO cartItem in shoppingCart.Items)
            {
                ProductDTO? catalogProduct = products.FirstOrDefault(p => p.Id == cartItem.IdProduct);

                if (catalogProduct.Price != cartItem.Price)
                {
                    var msgErro = $"The price of product {cartItem.Name} has changed (from: " +
                                  $"{string.Format("{0:C}", cartItem.Price)} to: " +
                                  $"{string.Format("{0:C}", catalogProduct.Price)}) since it has added to shoppingCart.";

                    AddError(msgErro);

                    ResponseResult? responseRemove = await _shopCartService.RemoveItem(cartItem.IdProduct);

                    if (ContainsErrors(responseRemove))
                    {
                        AddError($"It was not possible to auto remove the product {cartItem.Name} from your shopping cart, _" +
                                                   "remove and add it again.");
                        return false;
                    }

                    cartItem.Price = catalogProduct.Price;
                    var responseAdd = await _shopCartService.AddItem(cartItem);

                    if (ContainsErrors(responseAdd))
                    {
                        AddError($"It was not possible to auto update you product {cartItem.Name} from your shopping cart, _" +
                                                   "add it again.");
                        return false;
                    }

                    CleanErrors();
                    AddError(msgErro + " We've updated your shopping cart. Check it again.");

                    return false;
                }
            }

            return true;
        }
    }

    public static class OrderDTOExtension
    {
        public static void AssignOrderDTO(this OrderDTO order, CartDTO cartDTO, AddressDTO addressDTO)
        {
            order.Voucher = cartDTO.Voucher?.Code;
            order.HasVoucher = cartDTO.HasVoucher;
            order.Amount = cartDTO.Total;
            order.Discount = cartDTO.Discount;
            order.OrderItems = cartDTO.Items;

            order.Address = addressDTO;
        }
    }
}
