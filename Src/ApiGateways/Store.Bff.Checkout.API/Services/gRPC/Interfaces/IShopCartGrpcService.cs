using Store.Bff.Checkout.Models;

namespace Store.Bff.Checkout.API.Services.gRPC.Interfaces
{
    public interface IShopCartGrpcService
    {
        Task<CartDTO> GetCart();

    }
}
