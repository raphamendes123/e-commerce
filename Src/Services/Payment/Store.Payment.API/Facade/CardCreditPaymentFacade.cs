using Core.ApiConfigurations.Configurations;
using Microsoft.Extensions.Options;
using Store.Payment.API.Domain.Data.Entitys;
using Store.Payment.API.Domain.Data.Repository.Interfaces;
using Store.Payment.API.Extensions;
using Store.Payment.API.Facade.Interfaces;
using Store.Payment.Pay;
using Store.Payment.Pay.Enums;

namespace Store.Payment.API.Facade
{
    public class CardCreditPaymentFacade : IPaymentFacade
    {
        private readonly PaymentSettings _paymentSettings;

        public CardCreditPaymentFacade(IOptions<PaymentSettings> paymentSettings)
        {
            _paymentSettings = paymentSettings.Value;
        }

        public async Task<Domain.Data.Entitys.TransactionEntity> AuthorizePayment(PaymentEntity payment)
        {
            var payService =
               new PayService(
                   apiKey: _paymentSettings.ApiKey,
                   encryptionKey: _paymentSettings.EncryptionKey);

            var cardHash = new CardHash(payService)
            {
                CardNumber = payment.CreditCard.CardNumber,
                CardHolderName = payment.CreditCard.Holder,
                CardExpirationDate = payment.CreditCard.ExpirationDate,
                CardCvv = payment.CreditCard.SecurityCode,
            };
            
            var cardHashEncrypto = cardHash.Encrypto();

            var transaction = new Pay.Transaction(payService)
            { 
                CardHash = cardHashEncrypto,
                CardNumber = payment.CreditCard.CardNumber,
                CardHolderName = payment.CreditCard.Holder,
                CardExpirationDate = payment.CreditCard.ExpirationDate,
                PaymentMethod = EnumPaymentMethod.CreditCard,
                Amount = payment.Amount,
            };
            
            return (await transaction.AuthorizeCardTransaction()).ToTransactionEntity();
        }

        public async Task<TransactionEntity> CancelAuthorization(TransactionEntity transaction)
        {
            var payService =
              new PayService(
                  apiKey: _paymentSettings.ApiKey,
                  encryptionKey: _paymentSettings.EncryptionKey);


            var tr = transaction.ToTransaction(payService);

            return (await tr.CancelledAuthorization()).ToTransactionEntity();
        }

        public async  Task<TransactionEntity> CapturePayment(TransactionEntity transaction)
        {
            var payService =
               new PayService(
                   apiKey: _paymentSettings.ApiKey,
                   encryptionKey: _paymentSettings.EncryptionKey);

           var tr = transaction.ToTransaction(payService);

            return (await tr.CaptureCardTransaction()).ToTransactionEntity();
        }
    }
}
