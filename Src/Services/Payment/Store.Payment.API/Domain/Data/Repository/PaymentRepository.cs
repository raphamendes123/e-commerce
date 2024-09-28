using Core.Domain.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Store.Payment.API.Domain.Data.Contexts;
using Store.Payment.API.Domain.Data.Entitys;
using Store.Payment.API.Domain.Data.Repository.Interfaces;

namespace Store.Payment.API.Domain.Data.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void AddPayment(PaymentEntity payment)
        {
            _context.Payments.Add(payment);
        }

        public void AddTransaction(TransactionEntity transaction)
        {
            _context.Transactions.Add(transaction);
        }

        public async Task<PaymentEntity> GetPaymentByIdOrder(Guid orderId)
        {
            return await _context.Payments.AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdOrder == orderId);
        }

        public async Task<IEnumerable<TransactionEntity>> GetTransactionsByIdOrder(Guid orderId)
        {
            return await _context.Transactions.AsNoTracking()
                .Where(t => t.Payment.IdOrder == orderId).ToListAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
