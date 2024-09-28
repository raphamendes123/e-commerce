using Store.ShopCart.API.Domain.Data.Entitys;

namespace Store.ShopCart.API.Protos
{
    public static class CartExtension
    {
        public static ResponseCart ToResponseCartPhoto(this Cart cart)
        {
            var responseGetCart = new ResponseCart
            {
                Id = cart.Id.ToString(),
                Idcustomer = cart.IdCustomer.ToString(),
                Total = (double)cart.Total,
                Discount = (double)cart.Discount,
                Hasvoucher = cart.HasVoucher,
            };

            if (cart.Voucher?.Code != null)
            {
                responseGetCart.Voucher = new ResponseVoucher
                {
                    Code = cart.Voucher.Code,
                    Percentage = (double?)cart.Voucher.Percentage ?? 0,
                    Discount = (double?)cart.Voucher.Discount ?? 0,
                    DiscountType = (int)cart.Voucher.DiscountType
                };
            }

            foreach (var item in cart.Items)
            {
                responseGetCart.Items.Add(new ResponseCartItem
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Image = item.Image,
                    Idproduct = item.IdProduct.ToString(),
                    Quantity = item.Quantity,
                    Price = (double)item.Price
                });
            }

            return responseGetCart;
        }
    }
}
