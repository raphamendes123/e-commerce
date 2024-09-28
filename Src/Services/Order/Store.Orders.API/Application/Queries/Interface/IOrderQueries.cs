using Store.Orders.Domain.DTOs;

namespace Store.Orders.API.Application.Queries.Interface
{
    public interface IOrderQuerie
    {
        Task<OrderDTO> GetLastOrder(Guid idCustomer);
        Task<IEnumerable<OrderDTO>> GetByIdCustomer(Guid idCustomer);
        Task<OrderDTO> GetAuthorizedOrders();
    }
}
