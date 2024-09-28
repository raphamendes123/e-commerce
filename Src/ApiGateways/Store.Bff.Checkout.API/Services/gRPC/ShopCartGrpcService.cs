using Store.Bff.Checkout.API.Protos;
using Store.Bff.Checkout.API.Services.gRPC.Interfaces;
using Store.Bff.Checkout.Models;
using Store.Orders.Domain.Enums;

namespace Store.Bff.Checkout.API.Services.gRPC
{
    public class ShopCartGrpcService : IShopCartGrpcService
    {
        private readonly CartProto.CartProtoClient _cartProtoClient;

        public ShopCartGrpcService(CartProto.CartProtoClient cartProtoClient)
        {
            _cartProtoClient = cartProtoClient;
        }

        public async Task<CartDTO> GetCart()
        {
            var response = await _cartProtoClient.GetCartAsync( new RequestGetCart());

            return MapCartDTO(response);
        }
        public async Task<int> GetCartQuantity()
        {
            var response = await _cartProtoClient.GetCartAsync(new RequestGetCart());

            var cartDTO = MapCartDTO(response);

            return (cartDTO?.Items.Sum(x => x.Quantity) ?? 0);
        }

        private static CartDTO MapCartDTO(ResponseCart cart)
        {
            var cartDto = new CartDTO
            {
                Total = (decimal)cart.Total,
                Discount = (decimal)cart.Discount,
                HasVoucher = cart.Hasvoucher
            };

            if (cart.Voucher != null)
            {
                cartDto.Voucher = new VoucherDTO
                {
                    Code = cart.Voucher.Code,
                    Percentage = (decimal?)cart.Voucher.Percentage,
                    Discount = (decimal?)cart.Voucher.Discount,
                    DiscountType = (EnumVoucherDiscountType)cart.Voucher.DiscountType
                };
            }

            foreach (var item in cart.Items)
            {
                cartDto.Items.Add(new CartItemDTO
                {
                    Name = item.Name,
                    Image = item.Image,
                    IdProduct = Guid.Parse(item.Idproduct),
                    Quantity = item.Quantity,
                    Price = (decimal)item.Price
                });
            }

            return cartDto;
        }
    }
}
