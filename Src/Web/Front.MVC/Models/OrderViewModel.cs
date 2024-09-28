namespace Front.MVC.Models
{
    public class OrderViewModel
    {
        #region Order
        public int Code { get; set; }

        // Authorized = 1,
        // Paid = 2,
        // Refused = 3,
        // Chargeback = 4,
        // Canceled = 5
        public int Status { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public decimal Discount { get; set; }
        public bool HasVoucher { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();

        #endregion

        #region Order Item

        public class OrderItemViewModel
        {
            public Guid OrderId { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public string Image { get; set; }
        }

        #endregion

        #region Address

        public AddressViewModel Address { get; set; }

        #endregion
    }
}
