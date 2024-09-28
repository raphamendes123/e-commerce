namespace Store.Bff.Checkout.Models
{
    public class CartItemDTO
    {
        public Guid IdProduct { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
