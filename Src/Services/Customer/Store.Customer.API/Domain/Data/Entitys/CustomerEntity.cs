using Core.Domain.Repository.Data;
using Core.Domain.Repository.DomainObjects;

namespace Store.Customer.API.Domain.Data.Entitys
{
    public class CustomerEntity : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Cpf Cpf { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        public AddressEntity Address { get; private set; }

        //EF relation
        protected CustomerEntity()
        {

        }

        public CustomerEntity(Guid id, string name, string email, string cpf)
        {
            Id = id;
            Name = name;
            Email = new Email(email);
            Cpf = new Cpf(cpf);
        }

        public void ReplaceEmail(string email)
        {
            Email = new Email(email);
        }

        public void ReplaceAddress(AddressEntity address)
        {
            Address = address;
        }
    }
}
