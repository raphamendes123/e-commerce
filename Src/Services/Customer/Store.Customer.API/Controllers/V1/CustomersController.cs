using Core.Mediator;
using Store.Customer.API.Application.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.ApiConfigurations;
using Store.Customer.API.Domain.Data.Repository.Interfaces;

namespace Store.Customer.API.Controllers.V1
{
  
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomersController : MainControllerApi
    {
        private readonly IMediatorHandler _mediator;
        private readonly ICustomerRepository _customerRepository;
        public CustomersController(IAspNetUser aspNetUser, IMediatorHandler mediatorHandler, ICustomerRepository customerRepository) : base(aspNetUser)
        {
            _mediator = mediatorHandler;
            _customerRepository = customerRepository;
        }

        [HttpGet("address")]
        public async Task<IActionResult> GetAddress()
        {
            var address = await _customerRepository.GetAddressById(_aspNetUser.GetUserId());

            return address is null ? NotFound() : CustomResponse(address);
        }

        [HttpPost("address")]
        public async Task<IActionResult> AddAddress(RegisterAddressCommand command)
        {
            command.IdCustomer = _aspNetUser.GetUserId();
            return CustomResponse(await _mediator.SendCommand(command));
        }

        [HttpPut("address")]
        public async Task<IActionResult> UpdateAddress(UpdateAddressCommand command)
        {
            command.IdCustomer = _aspNetUser.GetUserId();
            return CustomResponse(await _mediator.SendCommand(command));
        }
    }
}
