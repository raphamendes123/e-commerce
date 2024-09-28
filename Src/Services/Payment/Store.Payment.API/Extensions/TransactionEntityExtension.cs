using Store.Payment.API.Domain.Data.Entitys;
using Store.Payment.API.Domain.Enums;
using Store.Payment.Pay; 

namespace Store.Payment.API.Extensions
{
    public static class TransactionEntityExtension
    {
        public static Domain.Data.Entitys.TransactionEntity ToTransactionEntity(this Pay.Transaction transaction)
        {
            return new Domain.Data.Entitys.TransactionEntity
            {
                Id = Guid.NewGuid(),
                TransactionStatus = (EnumTransactionStatus)transaction.Status,
                Amount = transaction.Amount,
                CreditCardCompany = transaction.CardBrand,
                AuthorizationCode = transaction.AuthorizationCode,
                TransactionCost = transaction.Cost,
                TransactionDate = transaction.TransactionDate,
                NSU = transaction.Nsu,
                TID = transaction.Tid
            };
        }
    }
}
