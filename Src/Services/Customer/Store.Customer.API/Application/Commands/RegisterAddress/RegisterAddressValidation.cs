using FluentValidation;

namespace Store.Customer.API.Application.Commands.RegisterAddress
{
    public class RegisterAddressValidation : AbstractValidator<RegisterAddressCommand>
    {
        public RegisterAddressValidation()
        {
            RuleFor(c => c.StreetAddress)
                .NotEmpty()
                .WithMessage("Street Address must be set");

            RuleFor(c => c.BuildingNumber)
                .NotEmpty()
                .WithMessage("Building number must be set");

            RuleFor(c => c.ZipCode)
                .NotEmpty()
                .WithMessage("Zip code must be set");

            RuleFor(c => c.Neighborhood)
                .NotEmpty()
                .WithMessage("Neighborhood must be set");

            RuleFor(c => c.City)
                .NotEmpty()
                .WithMessage("City must be set");

            RuleFor(c => c.State)
                .NotEmpty()
                .WithMessage("State must be set");
        }
    }
}
