using Core.Domain.ResponseResult;
using Microsoft.Extensions.Options;
using Configurations;
using Front.MVC.Extensions;
using Front.MVC.Models;
using Front.MVC.Services.Abstracts;
using Front.MVC.Services.Checkout.Interfaces;

namespace Front.MVC.Services.Checkout
{
    public class CheckoutService : Service, ICheckoutService
    {
        private readonly HttpClient _httpClient;

        public CheckoutService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.CheckoutUrl);
            _httpClient = httpClient;
        }
        public async Task<CartViewModel> GetCart()
        {
            HttpResponseMessage? response = await _httpClient.GetAsync("/api/v1/Checkout/cart");

            HandleResponseErrors(response);

            return await response.DeserializerObjectResponse<CartViewModel>();
        }

        public async Task<ResponseResult> AddItem(CartItemViewModel item)
        {
            HttpResponseMessage? response = await _httpClient.PostAsync("/api/v1/Checkout/cart/items", item.ToObjectJson());

            if (!HandleResponseErrors(response))
            {
                return await response.DeserializerObjectResponse<ResponseResult>();
            }

            return ReturnOK();
        }

        public async Task<ResponseResult> UpdateItem(Guid idProduct, CartItemViewModel item)
        {
            HttpResponseMessage? response = await _httpClient.PutAsync($"/api/v1/Checkout/cart/items/{idProduct}", item.ToObjectJson());

            if (!HandleResponseErrors(response))
            {
                return await response.DeserializerObjectResponse<ResponseResult>();
            }

            return ReturnOK();
        }


        public async Task<ResponseResult> RemoveItem(Guid idProduct)
        {
            HttpResponseMessage? response = await _httpClient.DeleteAsync($"/api/v1/Checkout/cart/items/{idProduct}");

            if (!HandleResponseErrors(response))
            {
                return await response.DeserializerObjectResponse<ResponseResult>();
            }

            return ReturnOK();
        }

        public async Task<int> GetCartItemsQuantity()
        {
            var response = await _httpClient.GetAsync("/api/v1/Checkout/cart/quantity");

            HandleResponseErrors(response);

            return await response.DeserializerObjectResponse<int>();
        }


        public async Task<ResponseResult> ApplyVoucher(string voucherCode)
        {
            var response = await _httpClient.PostAsync($"/api/v1/Checkout/cart/apply-voucher", new { Code = voucherCode }.ToObjectJson());

            if (!HandleResponseErrors(response))
            {
                return await response.DeserializerObjectResponse<ResponseResult>();
            }

            return ReturnOK();
        }

        public async Task<ResponseResult> FinishOrder(TransactionViewModel transaction)
        {
            var orderContent = GetContent(transaction);

            var response = await _httpClient.PostAsync("/api/v1/Checkout/Orders", orderContent);

            if (!HandleResponseErrors(response)) 
            { 
                return await response.DeserializerObjectResponse<ResponseResult>();
            }

            return ReturnOK();
        }

        public async Task<OrderViewModel> GetLastOrder()
        {
            var response = await _httpClient.GetAsync("/api/v1/Checkout/orders/last");

            HandleResponseErrors(response);

            return await response.DeserializerObjectResponse<OrderViewModel>();
        }

        public async Task<IEnumerable<OrderViewModel>> GetByOrders()
        {
            var response = await _httpClient.GetAsync("/api/v1/Checkout/orders/my-orders");

            HandleResponseErrors(response);

            return await response.DeserializerObjectResponse<IEnumerable<OrderViewModel>>();
        }

        public TransactionViewModel ToOrder(CartViewModel cart, AddressViewModel address)
        {
            var order = new TransactionViewModel
            {
                Amount = cart.Total,
                Items = cart.Items,
                Discount = cart.Discount,
                HasVoucher = cart.HasVoucher,
                Voucher = cart.Voucher?.Code
            };

            if (address != null)
            {
                order.Address = new AddressViewModel
                {
                    Id = address.Id,
                    StreetAddress = address.StreetAddress,
                    BuildingNumber = address.BuildingNumber,
                    Neighborhood = address.Neighborhood,
                    ZipCode = address.ZipCode,
                    SecondaryAddress = address.SecondaryAddress,
                    City = address.City,
                    State = address.State
                };
            }

            return order;
        }
    }
}
