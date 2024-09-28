using Core.Domain.ResponseResult;
using Front.MVC.Models;

namespace Front.MVC.Services.Checkout.Interfaces
{
    public interface ICheckoutService
    {
        Task<CartViewModel> GetCart();

        Task<ResponseResult> AddItem(CartItemViewModel item);

        Task<int> GetCartItemsQuantity();

        Task<ResponseResult> UpdateItem(Guid idProduct, CartItemViewModel item);

        Task<ResponseResult> RemoveItem(Guid idProduct);

        Task<ResponseResult> ApplyVoucher(string voucherCode);


        // Order
        Task<ResponseResult> FinishOrder(TransactionViewModel transaction);
        Task<OrderViewModel> GetLastOrder();
        Task<IEnumerable<OrderViewModel>> GetByOrders();
        TransactionViewModel ToOrder(CartViewModel cart, AddressViewModel address);
    }
}
