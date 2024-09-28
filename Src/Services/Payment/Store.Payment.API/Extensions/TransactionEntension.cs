using Core.ApiConfigurations.Configurations;
using Store.Payment.API.Domain.Data.Entitys;
using Store.Payment.Pay;
using Store.Payment.Pay.Enums;

namespace Store.Payment.API.Extensions
{
    public static class TransactionExtension
    {
        public static Transaction ToTransaction(this TransactionEntity transaction, PayService payService)
        {
            return new Transaction(payService)
            {
                Status = (EnumTransactionStatus)transaction.TransactionStatus,
                Amount = transaction.Amount,
                CardBrand = transaction.CreditCardCompany,
                AuthorizationCode = transaction.AuthorizationCode,
                Cost = transaction.TransactionCost,
                Nsu = transaction.NSU,
                Tid = transaction.TID
            };
        }
    }
}
