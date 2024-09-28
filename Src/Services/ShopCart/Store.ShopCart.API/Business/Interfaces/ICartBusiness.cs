using FluentValidation.Results;
using Store.ShopCart.API.Domain.Data.Entitys;

namespace Store.ShopCart.API.Business.Interfaces
{
    public interface ICartBusiness
    {
        Task<Cart> GetCart();

        Task<ICollection<string>> AddItem(CartItem item);

        Task<ICollection<string>> UpdateItem(Guid idProduct, CartItem item);

        Task<ICollection<string>> RemoveItem(Guid idProduct);

        Task<ICollection<string>> ApplyVoucher(Voucher voucher);
    }
}
