using Core.Domain.Repository.DomainObjects;
using Core.Message;

namespace Store.Customer.API.Application.Events
{
    public class RegisterCustomerEvent : Event
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Cpf Cpf { get; private set; }

        public RegisterCustomerEvent(Guid id, string name, Email email, Cpf cpf)
        {
            //mensagem base Command - vai receber a mesma informacao, caso precise utilizar
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
        }
    }
}
