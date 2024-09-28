﻿using Store.Bff.Checkout.API.Attributes;
using Store.Bff.Checkout.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Store.Bff.Checkout.API.Models
{
    public class OrderDTO
    {
        #region Order

        public int Code { get; set; }
        // Authorized = 1,
        // Paid = 2,
        // Refused = 3,
        // Delivered = 4,
        // Canceled = 5
        public int Status { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public decimal Discount { get; set; }
        public string Voucher { get; set; }
        public bool HasVoucher { get; set; }

        public List<CartItemDTO> OrderItems { get; set; }

        #endregion

        #region Address

        public AddressDTO Address { get; set; }

        #endregion

        #region Credit Card

        [Required(ErrorMessage = "Card number is required")]
        [DisplayName("Card number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Please inform card holder")]
        [DisplayName("Holder")]
        public string Holder { get; set; }

        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "The expiration must be in form of MM/YY")]
        [CreditCardExpired(ErrorMessage = "Expired Credit Card")]
        [Required(ErrorMessage = "Credit card expiration is required")]
        [DisplayName("Expiration MM/YY")]
        public string ExpirationDate { get; set; }

        [Required(ErrorMessage = "Security code is required")]
        [DisplayName("Security Code")]
        public string SecurityCode { get; set; }

        #endregion
    }
}
