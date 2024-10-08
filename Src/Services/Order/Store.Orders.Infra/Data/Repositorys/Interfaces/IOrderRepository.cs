using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Domain.Repository.Data;
using Store.Orders.Domain.Data.Entitys.Orders;

namespace Store.Orders.Infra.Data.Repositorys.Interfaces
{
    public interface IOrderRepository : IRepository<OrderEntity>
    {
        Task<OrderEntity> GetById(Guid id);
        Task<IEnumerable<OrderEntity>> GetCustomersById(Guid idCustomer);
        void Add(OrderEntity order);
        void Update(OrderEntity order);
        DbConnection GetConnection();
        Task<OrderEntity> GetLastOrder(Guid idCustomer);
        Task<OrderEntity> GetLastAuthorizedOrder();
        /* Order Item */
        Task<OrderItemEntity> GetItemById(Guid id);
        Task<OrderItemEntity> GetItemByOrder(Guid idOrder, Guid idProduct);
    }
}