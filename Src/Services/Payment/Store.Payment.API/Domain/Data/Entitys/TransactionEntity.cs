using Core.Domain.Repository.DomainObjects;
using Store.Payment.API.Domain.Enums;

namespace Store.Payment.API.Domain.Data.Entitys
{
    public class TransactionEntity : Entity
    {
        public string AuthorizationCode { get; set; }
        public string CreditCardCompany { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public decimal TransactionCost { get; set; }
        public EnumTransactionStatus TransactionStatus { get; set; }
        public string TID { get; set; } // Id - GATEWAY
        public string NSU { get; set; } // Meio (pagseguro,paypal)

        public Guid IdPayment { get; set; }

        // EF Relation
        public PaymentEntity Payment { get; set; }
    }
}
