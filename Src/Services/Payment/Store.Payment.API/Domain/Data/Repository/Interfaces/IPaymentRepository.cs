using Core.Domain.Repository.Data;
using Store.Payment.API.Domain.Data.Entitys;

namespace Store.Payment.API.Domain.Data.Repository.Interfaces
{
    public interface IPaymentRepository : IRepository<PaymentEntity>
    {
        void AddTransaction(TransactionEntity transaction);
        void AddPayment(PaymentEntity payment);
        Task<PaymentEntity> GetPaymentByIdOrder(Guid idOrder);
        Task<IEnumerable<TransactionEntity>> GetTransactionsByIdOrder(Guid idOrder);
    }
}
