using Core.Message;
using Store.Customer.API.Application.Events;
using Store.Customer.API.Domain.Data.Entitys;
using Store.Customer.API.Domain.Data.Repository.Interfaces;
using FluentValidation.Results;
using MediatR;
using Core.SpecificationsUseCase;
using Store.Customer.API.Domain.Data.Contexts;

namespace Store.Customer.API.Application.Commands
{
    public class RegisterCustomerHandler: CommandHandler, IRequestHandler<RegisterCustomerCommand, ValidationResult>
    {
        private readonly CustomerDbContext _customerDbContext;
        private readonly ICustomerRepository _customerRepository;

        public RegisterCustomerHandler(
            CustomerDbContext customerDbContex,
            ICustomerRepository customerRepository)
        {
            _customerDbContext = customerDbContex;
            _customerRepository = customerRepository;
        }

        public async Task<ValidationResult> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            CustomerEntity? customer = new CustomerEntity(id: request.Id, name: request.Name, email: request.Email.Address, cpf: request.Cpf.Number);
                       
            ValidationUseCase? validate = await new RegisterCustomerUseCase(_customerDbContext).ValidateAsync(customer);

            if (!validate.IsValid)
            {
                AddError(validate.Errors.ToValidationFailure());
                return ValidationResult;
            }

            _customerRepository.Add(customer);

            customer.AddEvent(new RegisterCustomerEvent(request.Id, request.Name, request.Email, request.Cpf));

            return await PersistData(_customerRepository.UnitOfWork);
        }  
    }
}
