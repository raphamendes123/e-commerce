using Core.ApiConfigurations;
using Core.Mediator;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using Store.Orders.API.Application.Commands.RegisterOrder;
using Store.Orders.API.Application.Queries.Interface;
using Store.Orders.Domain.DTOs;

namespace Store.Orders.API.Controllers.V1
{

    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OrdersController : MainControllerApi
    {
        private readonly IMediatorHandler _mediator; 
        private readonly IOrderQuerie _orderQueries;

        public OrdersController(IAspNetUser aspNetUser, IMediatorHandler mediator, IOrderQuerie orderQueries) : base(aspNetUser)
        {
            _mediator = mediator;
            _orderQueries = orderQueries; 
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOrderCommand(RegisterOrderCommand order)
        {
            //SETA O ID CUSTOMER VINCULADO NA REQUISICAO
            order.IdCustomer = _aspNetUser.GetUserId();
            return CustomResponse(await _mediator.SendCommand(order));
        }

        [HttpGet("last")]
        public async Task<ActionResult<OrderDTO>> LastOrder()
        {
            var order = await _orderQueries.GetLastOrder(_aspNetUser.GetUserId());

            return order == null ? NoContent() : CustomResponse(order);
        }

        [HttpGet("my-orders")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> MyOrders()
        {
            var orders = await _orderQueries.GetByIdCustomer(_aspNetUser.GetUserId());

            return orders == null ? NoContent() : CustomResponse(orders);
        }
    }
 
}
