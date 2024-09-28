using Core.Message;
using FluentValidation;
using Store.Orders.Domain.DTOs;

namespace Store.Orders.API.Application.Commands.RegisterOrder
{
    public class RegisterOrderCommand : Command
    { 
        // Order
        public Guid IdCustomer { get; set; }
        public decimal Amount { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }

        // Voucher
        public string Voucher { get; set; }
        public bool HasVoucher { get; set; }
        public decimal Discount { get; set; }

        // Address
        public AddressDTO Address { get; set; }

        // Cartao
        public string CardNumber { get; set; }
        public string Holder { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new RegisterOrderValidation().Validate(this);
            return ValidationResult.IsValid;
        } 
    }
}
