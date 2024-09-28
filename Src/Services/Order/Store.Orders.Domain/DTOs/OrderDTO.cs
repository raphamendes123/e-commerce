using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Orders.Domain.DTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public int Code { get; set; }

        public Guid IdCustomer { get; set; }
        public int Status { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public decimal Discount { get; set; }
        public string Voucher { get; set; }
        public bool HasVoucher { get; set; }

        public List<OrderItemDTO> OrderItems { get; set; }
        public AddressDTO Address { get; set; } 
    }
}
