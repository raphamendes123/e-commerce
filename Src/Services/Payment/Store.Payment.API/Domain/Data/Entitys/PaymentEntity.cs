using Core.Domain.Repository.Data;
using Core.Domain.Repository.DomainObjects;
using Store.Payment.API.Domain.Enums;
using Store.Payment.API.Domain.Models;

namespace Store.Payment.API.Domain.Data.Entitys
{
    public class PaymentEntity : Entity, IAggregateRoot
    {
        public PaymentEntity()
        {
            Transactions = new List<TransactionEntity>();
        }

        public Guid IdOrder { get; set; }
        public EnumTypePayment TypePayment { get; set; }
        public decimal Amount { get; set; }

        public CreditCard CreditCard { get; set; }

        // EF Relation
        public ICollection<TransactionEntity> Transactions { get; set; }

        public void AddTransaction(TransactionEntity transaction)
        {
            Transactions.Add(transaction);
        }
    }
}
