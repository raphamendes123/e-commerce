using Core.Domain.ResponseResult;
using Microsoft.Extensions.Options;
using Configurations;
using Store.Bff.Checkout.Models;
using Store.Bff.Checkout.Extensions;
using Store.Bff.Checkout.Services;
using Store.Bff.Checkout.API.Services.Rest.ShopCart.Interfaces;

namespace Store.Bff.Checkout.API.Services.Rest.ShopCart
{
    public class ShopCartService : Service, IShopCartService
    {
        private readonly HttpClient _httpClient;

        public ShopCartService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.ShopCartUrl);
            _httpClient = httpClient;
        }
        public async Task<CartDTO> GetCart()
        {
            HttpResponseMessage? response = await _httpClient.GetAsync("/api/v1/cart");

            ResponseErrors(response);

            return await response.DeserializerResponse<CartDTO>();
        }

        public async Task<ResponseResult> AddItem(CartItemDTO item)
        {
            HttpResponseMessage? response = await _httpClient.PostAsync("/api/v1/cart", item.ToJson());

            if (!ResponseErrors(response))
            {
                return await response.DeserializerResponse<ResponseResult>();
            }

            return ReturnOK();
        }

        public async Task<ResponseResult> UpdateItem(Guid idProduct, CartItemDTO item)
        {
            HttpResponseMessage? response = await _httpClient.PutAsync($"/api/v1/cart/{idProduct}", item.ToJson());

            if (!ResponseErrors(response))
            {
                return await response.DeserializerResponse<ResponseResult>();
            }

            return ReturnOK();
        }


        public async Task<ResponseResult> RemoveItem(Guid idProduct)
        {
            HttpResponseMessage? response = await _httpClient.DeleteAsync($"/api/v1/cart/{idProduct}");

            if (!ResponseErrors(response))
            {
                return await response.DeserializerResponse<ResponseResult>();
            }

            return ReturnOK();
        }

        public async Task<ResponseResult> ApplyVoucher(VoucherDTO voucher)
        {
            HttpResponseMessage? response = await _httpClient.PostAsync("/api/v1/cart/apply-voucher", voucher.ToJson());

            if (!ResponseErrors(response))
            {
                return await response.DeserializerResponse<ResponseResult>();
            }

            return ReturnOK();
        }

        public async Task<int> GetCartItemsQuantity()
        {
            var response = await _httpClient.GetAsync("/api/v1/cart/quantity/");

            ResponseErrors(response);

            return await response.DeserializerResponse<int>();
        }
    }
}
