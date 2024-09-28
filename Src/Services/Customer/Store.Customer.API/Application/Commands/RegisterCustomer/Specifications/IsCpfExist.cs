using Store.Customer.API.Domain.Data.Entitys;
using Microsoft.EntityFrameworkCore;
using Core.SpecificationsUseCase.Interface;
using Store.Customer.API.Domain.Data.Contexts;

namespace Store.Customer.API.Application.Commands.RegisterStore.Customer.Specifications
{
    public class IsCpfExist : ISpecification<CustomerEntity>
    {
        private readonly CustomerDbContext _context;

        public IsCpfExist(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsSatisfiedBy(CustomerEntity entity)
        {
            CustomerEntity? customer = await _context.Customers.FirstOrDefaultAsync(x => x.Cpf.Number == entity.Cpf.Number);

            return customer?.Id != null;
        }
    }
}
