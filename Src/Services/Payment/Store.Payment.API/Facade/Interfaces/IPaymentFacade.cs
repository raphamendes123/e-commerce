using Store.Payment.API.Domain.Data.Entitys;

namespace Store.Payment.API.Facade.Interfaces
{
    public interface IPaymentFacade
    {
        Task<TransactionEntity> AuthorizePayment(PaymentEntity payment);
        Task<TransactionEntity> CapturePayment(TransactionEntity transaction);
        Task<TransactionEntity> CancelAuthorization(TransactionEntity transaction);
    }
}
