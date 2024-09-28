using Core.SpecificationsUseCase;
using Store.Customer.API.Application.Commands.RegisterStore.Customer.Specifications;
using Store.Customer.API.Domain.Data.Contexts;
using Store.Customer.API.Domain.Data.Entitys;

namespace Store.Customer.API.Application.Commands
{
    public class RegisterCustomerUseCase : Validator<CustomerEntity>
    {
        private readonly CustomerDbContext _context;

        public RegisterCustomerUseCase(CustomerDbContext context)
        {
            Add("IsCpfExist",new Rule<CustomerEntity>(new IsCpfExist(context), "Existing CPF"));
        }
    }
}
