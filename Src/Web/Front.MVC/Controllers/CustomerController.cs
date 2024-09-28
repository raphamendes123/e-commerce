using Core.ApiConfigurations;
using Core.Domain.ResponseResult;
using Front.MVC.Models;
using Front.MVC.Services.Customer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Front.MVC.Controllers
{
    [Authorize]
    public class CustomerController : MainControllerMvc
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> NewAddress(AddressViewModel address)
        {
            ResponseResult response;

            if (address.Id == Guid.Empty)
                response = await _customerService.AddAddress(address);
            else
                response = await _customerService.UpdateAddress(address);

            if (HasErrors(response)) 
                TempData["Errors"] = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

            return RedirectToAction("DeliveryAddress", "Order");
        }
    }
}
