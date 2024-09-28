using Store.Orders.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Orders.Domain.DTOs
{
    public class VoucherDTO
    {
        public string? Code { get; set; } 
        public decimal? Percentage { get; set; }
        public decimal? Discount { get; set; }
        public VoucherDiscountType DiscountType { get; set; }
    }
 
}
