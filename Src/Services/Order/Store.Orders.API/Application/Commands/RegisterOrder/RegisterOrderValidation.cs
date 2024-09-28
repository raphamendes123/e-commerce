using FluentValidation;

namespace Store.Orders.API.Application.Commands.RegisterOrder
{
    public class RegisterOrderValidation : AbstractValidator<RegisterOrderCommand>
    {
        public RegisterOrderValidation()
        {
            RuleFor(c => c.IdCustomer)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid customer id");

            RuleFor(c => c.OrderItems.Count)
                .GreaterThan(0)
                .WithMessage("The order needs to have at least 1 item");

            RuleFor(c => c.Amount)
                .GreaterThan(0)
                .WithMessage("Invalid order amount");

            RuleFor(c => c.CardNumber)
                .CreditCard()
                .WithMessage("Invalid credit card");

            RuleFor(c => c.Holder)
                .NotNull()
                .WithMessage("Holder name is required.");

            RuleFor(c => c.SecurityCode.Length)
                .GreaterThan(2)
                .LessThan(5)
                .WithMessage("The security code must have at least 3 or 4 numbers.");

            RuleFor(c => c.ExpirationDate)
                .NotNull()
                .WithMessage("Expiration date is required.");
        }
    }
}
