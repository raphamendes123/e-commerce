using Store.Orders.API.Application.Queries.Interface;
using Store.Orders.Domain.DTOs;
using Store.Orders.Domain.Extensions; 
using Store.Orders.Infra.Data.Repositorys.Interfaces;

namespace Store.Orders.API.Application.Queries
{
    public class OrderQuerie : IOrderQuerie
    {
        private readonly IOrderRepository _orderRepository;

        public OrderQuerie(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDTO> GetLastOrder(Guid idCustomer)
        {
            var order = await _orderRepository.GetLastOrder(idCustomer);

            if (order is null)
                return null;

            return order.ToOrderDTO();
        }

        public async Task<IEnumerable<OrderDTO>> GetByIdCustomer(Guid idCustomer)
        {
            var orders = await _orderRepository.GetCustomersById(idCustomer);

            if (orders is null)
                return null;

            return orders.Select( x => x.ToOrderDTO());
        }

        public async Task<OrderDTO> GetAuthorizedOrders()
        {
            var order = await _orderRepository.GetLastAuthorizedOrder();
            
            if (order is null)
                return null;

            return order.ToOrderDTO();
        } 
    }
}
