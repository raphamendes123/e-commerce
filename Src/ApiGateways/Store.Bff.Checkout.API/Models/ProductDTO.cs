namespace Store.Bff.Checkout.Models
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }

        private bool IsAvailable(int quantity)
        {
            return Active && Stock >= quantity;
        }

        public ICollection<string> IsValid()
        {
            ICollection<string> strings = new HashSet<string>();

            if (this == null)
            {
                strings.Add("Product does not exist.");
            } 

            return strings;
        }
        public ICollection<string> IsValid(CartItemDTO cartItemDTO)
        {
            ICollection<string> strings = new HashSet<string>();

            if (this == null)
            {
                strings.Add("Product does not exist.");
            }

            if (!IsAvailable(cartItemDTO.Quantity))
            {
                strings.Add("Quantity out of stock or product not active.");
            }

            return strings;
        }
    }


}
