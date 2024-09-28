using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Store.Orders.Domain.Data.Entitys.Vouchers;
using Store.Orders.Infra.Data.Contexts;
using Store.Orders.Infra.Data.Repositorys.Interfaces;

namespace Store.Orders.Infra.Data.Repositorys
{
    public class VoucherRepository : IVoucherRepository
    {
        public readonly OrdersDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public VoucherRepository(OrdersDbContext context)
        {
            _context = context;
        }

        public void Add(VoucherEntity model)
        {
            _context.Vouchers.Add(model);
        }

        public async Task<IEnumerable<VoucherEntity>> GetAllAsync()
        {
            return await _context.Vouchers.AsNoTracking().ToListAsync();
        }

        public async Task<VoucherEntity> GetIdAsync(Guid id)
        {
            return await _context.Vouchers.FindAsync(id);
        }

        public void Update(VoucherEntity model)
        {
            _context.Vouchers.Update(model);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public async Task<VoucherEntity> GetCodeAsync(string code)
        {
            return await _context.Vouchers.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code);
        }
    }
}
