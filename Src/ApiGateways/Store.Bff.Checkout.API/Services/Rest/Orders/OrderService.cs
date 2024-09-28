using Configurations;
using Core.Domain.ResponseResult;
using Microsoft.Extensions.Options;
using Store.Bff.Checkout.API.Models;
using Store.Bff.Checkout.API.Services.Rest.Orders.Interfaces;
using Store.Bff.Checkout.Extensions;
using Store.Bff.Checkout.Models;
using Store.Bff.Checkout.Services;
using System.Net;

namespace Store.Bff.Checkout.API.Services.Rest.Orders
{
    public class OrderService : Service, IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.OrderUrl);
            _httpClient = httpClient;
        }

        public async Task<OrderDTO> GetLastOrder()
        {
            HttpResponseMessage? response = await _httpClient.GetAsync("/api/v1/orders/last");

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            ResponseErrors(response);

            return await response.DeserializerResponse<OrderDTO>();
        }

        public async Task<IEnumerable<OrderDTO>> GetOrders()
        {
            var response = await _httpClient.GetAsync("/api/v1/orders/my-orders");

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            ResponseErrors(response);

            return await response.DeserializerResponse<IEnumerable<OrderDTO>>();
        }

        public async Task<VoucherDTO> GetVoucherCodeAsync(string code)
        {
            HttpResponseMessage? response = await _httpClient.GetAsync($"/api/v1/voucher/{code}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            ResponseErrors(response);

            return await response.DeserializerResponse<VoucherDTO>();
        }

        public async Task<ResponseResult> FinishOrder(OrderDTO order)
        {
            var orderContent = GetContent(order);

            var response = await _httpClient.PostAsync("/api/v1/orders", orderContent);

            if (!ResponseErrors(response))
                return await response.DeserializerResponse<ResponseResult>();

            return ReturnOK();
        }
    }
}
