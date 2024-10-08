using System;
using Core.Domain.Repository.DomainObjects;

namespace Store.Orders.Domain.Data.Entitys.Orders
{
    public class OrderItemEntity : Entity
    {
        public Guid IdOrder { get; private set; }
        public Guid IdProduct { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public string ProductImage { get; set; }

        // EF Rel.
        public OrderEntity Order { get; set; }

        public OrderItemEntity(Guid idProduct,
            string productName,
            int quantity,
            decimal price,
            string productImage = null)
        {
            IdProduct = idProduct;
            ProductName = productName;
            Quantity = quantity;
            Price = price;
            ProductImage = productImage;
        }

        // EF ctor
        protected OrderItemEntity() { }

        internal decimal CalculateAmount()
        {
            return Quantity * Price;
        }
    }
}