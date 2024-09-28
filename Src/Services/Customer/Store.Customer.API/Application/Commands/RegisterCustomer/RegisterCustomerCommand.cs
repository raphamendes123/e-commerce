using Core.Domain.Repository.DomainObjects;
using Core.Message;

namespace Store.Customer.API.Application.Commands
{
    public class RegisterCustomerCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Cpf Cpf { get; private set; }

        public RegisterCustomerCommand(Guid id, string name,string email, string cpf)
        {
            //mensagem base Command - vai receber a mesma informacao, caso precise utilizar
            AggregateId = id;
            Id = id;
            Name = name;
            Email = new Email(email);
            Cpf = new Cpf(cpf);
        }

        public override bool IsValid()
        {
            return new RegisterCustomerValidation().Validate(this).IsValid;
        }
    }
}
