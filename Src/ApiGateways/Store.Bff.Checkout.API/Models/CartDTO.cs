using System;
using System.Collections.Generic;

namespace Store.Bff.Checkout.Models
{
    public class CartDTO
    {
        public decimal Total { get; set; }
        public VoucherDTO? Voucher { get; set; }
        public bool HasVoucher { get; set; }
        public decimal Discount { get; set; }
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
    }

    
}