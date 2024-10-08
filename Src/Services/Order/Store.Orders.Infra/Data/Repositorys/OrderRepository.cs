using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Store.Orders.Domain.Data.Entitys.Orders;
using Store.Orders.Domain.Enums;
using Store.Orders.Infra.Data.Contexts;
using Store.Orders.Infra.Data.Repositorys.Interfaces;

namespace Store.Orders.Infra.Data.Repositorys
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersDbContext _context;

        public OrderRepository(OrdersDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public DbConnection GetConnection() => _context.Database.GetDbConnection();

        public async Task<OrderEntity> GetById(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<OrderEntity>> GetCustomersById(Guid idCustomer)
        {
            return await _context.Orders
                .Include(p => p.OrderItems)
                .AsNoTracking()
                .Where(p => p.IdCustomer == idCustomer)
                .ToListAsync();
        }

        public void Add(OrderEntity order)
        {
            _context.Orders.Add(order);
        }

        public void Update(OrderEntity order)
        {
            _context.Orders.Update(order);
        }


        public async Task<OrderItemEntity> GetItemById(Guid id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        public async Task<OrderItemEntity> GetItemByOrder(Guid idOrder, Guid idProduct)
        {
            return await _context.OrderItems
                .FirstOrDefaultAsync(p => p.IdProduct == idProduct && p.IdOrder == idOrder);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<OrderEntity> GetLastOrder(Guid idCustomer)
        {
            var fiveMinutesAgo = DateTime.Now.AddMinutes(-5);

            return _context.Orders
                .Include(i => i.OrderItems)
                .AsNoTracking()
                .Where(o => o.IdCustomer == idCustomer && o.DateAdded > fiveMinutesAgo && o.DateAdded <= DateTime.Now)
                .OrderByDescending(o => o.DateAdded).FirstOrDefaultAsync();
        }

        public Task<OrderEntity> GetLastAuthorizedOrder()
        {
            return _context.Orders.Include(i => i.OrderItems)
                .Where(o => o.OrderStatus == OrderStatus.Authorized)
                .OrderBy(o => o.DateAdded).FirstOrDefaultAsync();


        }
    }
}