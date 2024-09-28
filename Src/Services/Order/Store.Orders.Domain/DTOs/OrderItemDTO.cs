using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Orders.Domain.DTOs
{
    public class OrderItemDTO
    {
        public Guid IdOrder { get; set; }
        public Guid IdProduct { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; } 
    }
}
