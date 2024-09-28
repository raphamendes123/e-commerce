using Core.Domain.Repository.Data;
using Store.Customer.API.Domain.Data.Entitys;
using Store.Customer.API.Domain.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Store.Customer.API.Domain.Data.Contexts;

namespace Store.Customer.API.Domain.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        public readonly CustomerDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public void Add(CustomerEntity entity)
        {
            _context.Customers.Add(entity);
        }

        public async Task<IEnumerable<CustomerEntity>> GetAllAsync()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<CustomerEntity> GetIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public void Update(CustomerEntity entity)
        {
            _context.Customers.Update(entity);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<AddressEntity> GetAddressById(Guid idCustomer)
        {
            return await _context.Addresses.FirstOrDefaultAsync(x => x.CustomerId == idCustomer);
        }

        public void AddAddress(AddressEntity entity)
        {
            _context.Addresses.Add(entity);
        }
        public void UpdateAddress(AddressEntity entity)
        {
            _context.Addresses.Update(entity);
        }
    }
}
