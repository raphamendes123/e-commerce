using Core.Domain.Repository.DomainObjects;
using FluentValidation;

namespace Store.Customer.API.Application.Commands
{
    public class RegisterCustomerValidation : AbstractValidator<RegisterCustomerCommand>
    {
        public RegisterCustomerValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid customer id");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Customer name must be set");

            RuleFor(c => c.Cpf.Number)
                .Must(IsValidCpf)
                .WithMessage("Cpf Invalid.");

            RuleFor(c => c.Email.Address)
                .Must(HasValidEmail)
                .WithMessage("Wrong e-mail.");
        }

        protected static bool IsValidCpf(string cpf)
        {
            return Cpf.IsValid(cpf);
        }

        protected static bool HasValidEmail(string email)
        {
            return Email.IsValid(email);
        }
    }
   
}
