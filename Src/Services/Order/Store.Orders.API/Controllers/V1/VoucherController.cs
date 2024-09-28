using Core.ApiConfigurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Store.Orders.API.Application.Queries.Interface;
using Store.Orders.Domain.DTOs;
using Store.Orders.Infra.Data.Contexts; 
using System.Net;
using System.Runtime.CompilerServices;

namespace Store.ShopCart.API.Controllers.V1
{

    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class VoucherController : MainControllerApi
    {
        private readonly OrdersDbContext _context;
        private readonly IVoucherQuerie _voucherQuerie;

        public VoucherController(IAspNetUser aspNetUser, OrdersDbContext context, IVoucherQuerie voucherQuerie) : base(aspNetUser)
        {
            _context = context;
            _voucherQuerie = voucherQuerie;
        }

        [HttpGet("{code}")]
        [ProducesResponseType(typeof(VoucherDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCart(string code)
        {
            if(code.IsNullOrEmpty()) return NotFound();

            var voucher = await _voucherQuerie.GetCodeAsync(code);

            return voucher == null ? NotFound() : CustomResponse(voucher);
        } 
    }
}
