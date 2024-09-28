using Core.Domain.ResponseResult;
using Store.Bff.Checkout.Models;


namespace Store.Bff.Checkout.API.Services.Rest.ShopCart.Interfaces
{
    public interface IShopCartService
    {
        Task<CartDTO> GetCart();

        Task<ResponseResult> AddItem(CartItemDTO item);

        Task<int> GetCartItemsQuantity();

        Task<ResponseResult> UpdateItem(Guid idProduct, CartItemDTO item);

        Task<ResponseResult> RemoveItem(Guid idProduct);

        Task<ResponseResult> ApplyVoucher(VoucherDTO voucher);

    }
}
