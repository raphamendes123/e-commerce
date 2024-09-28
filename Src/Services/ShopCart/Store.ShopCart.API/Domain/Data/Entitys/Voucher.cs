namespace Store.ShopCart.API.Domain.Data.Entitys
{
    public class Voucher
    {
        public decimal? Percentage { get; set; }
        public decimal? Discount { get; set; }
        public string? Code { get; set; }
        public DiscountType? DiscountType { get; set; } = null;
    }

    public enum DiscountType
    {
        Percentage = 0,
        Value = 1
    }
}