using Core.Domain.Repository.Data;
using Store.Customer.API.Domain.Data.Entitys;

namespace Store.Customer.API.Domain.Data.Repository.Interfaces
{
    public interface ICustomerRepository : IRepository<CustomerEntity>
    {
        Task<IEnumerable<CustomerEntity>> GetAllAsync();

        Task<CustomerEntity> GetIdAsync(Guid id);

        void Add(CustomerEntity entity);

        void Update(CustomerEntity entity);

        Task<AddressEntity> GetAddressById(Guid idCustomer);
        void AddAddress(AddressEntity entity);
        void UpdateAddress(AddressEntity entity);
    }
}
