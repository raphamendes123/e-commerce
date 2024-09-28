using FluentValidation;

namespace Store.Customer.API.Application.Commands.UpdateAddress
{
    public class UpdateAddressValidation : AbstractValidator<UpdateAddressCommand>
    {
        public UpdateAddressValidation()
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
