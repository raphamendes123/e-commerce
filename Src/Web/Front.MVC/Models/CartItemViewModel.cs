namespace Front.MVC.Models
{
    public class CartItemViewModel
    {
        public Guid IdProduct { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}