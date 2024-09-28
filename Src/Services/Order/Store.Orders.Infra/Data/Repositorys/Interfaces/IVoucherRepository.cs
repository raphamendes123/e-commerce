using Core.Domain.Repository.Data;
using Store.Orders.Domain.Data.Entitys.Vouchers;

namespace Store.Orders.Infra.Data.Repositorys.Interfaces
{
    public interface IVoucherRepository : IRepository<VoucherEntity>
    {
        Task<IEnumerable<VoucherEntity>> GetAllAsync();

        Task<VoucherEntity> GetIdAsync(Guid id);

        Task<VoucherEntity> GetCodeAsync(string code);

        void Add(VoucherEntity model);

        void Update(VoucherEntity model);
    }
}