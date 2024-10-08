using System;
using System.Collections.Generic;

namespace Front.MVC.Models
{
    public class CartViewModel
    {
        public decimal Total { get; set; }
        public VoucherViewModel Voucher { get; set; }
        public bool HasVoucher { get; set; }
        public decimal Discount { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
    }
}