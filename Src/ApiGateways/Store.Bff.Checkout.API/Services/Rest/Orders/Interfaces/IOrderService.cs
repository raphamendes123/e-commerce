using Core.Domain.ResponseResult;
using Store.Bff.Checkout.API.Models;
using Store.Bff.Checkout.Models;

namespace Store.Bff.Checkout.API.Services.Rest.Orders.Interfaces
{
    public interface IOrderService
    {
        Task<VoucherDTO> GetVoucherCodeAsync(string code);
        Task<IEnumerable<OrderDTO>> GetOrders();
        Task<OrderDTO> GetLastOrder();
        Task<ResponseResult> FinishOrder(OrderDTO order);
    }
}
