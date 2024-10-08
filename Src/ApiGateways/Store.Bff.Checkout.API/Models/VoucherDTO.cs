using Store.Orders.Domain.Enums;
using System.Text.Json.Serialization;

namespace Store.Bff.Checkout.Models
{
    public class VoucherDTO
    {
        public string? Code { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? Discount { get; set; }
         
        public EnumVoucherDiscountType? DiscountType { get; set; }
    }
}