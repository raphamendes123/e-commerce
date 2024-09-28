using Core.ApiConfigurations;
using EasyNetQ.Events;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Store.ShopCart.API.Domain.Data.Contexts;
using Store.ShopCart.API.Domain.Data.Entitys;
using Store.ShopCart.API.Protos;

namespace Store.ShopCart.API.Services.gRPC
{
    [Authorize]
    public class CartGrpcService : CartProto.CartProtoBase
    {
        private readonly ILogger<CartGrpcService> _logger;
        private readonly IAspNetUser _aspNetUser;
        private readonly CartDbContext _context;

        public CartGrpcService(
            ILogger<CartGrpcService> logger, 
            IAspNetUser aspNetUser, 
            CartDbContext context)
        {
            _logger = logger;
            _aspNetUser = aspNetUser;
            _context = context;
        }

        public override async Task<ResponseCart> GetCart(RequestGetCart request, ServerCallContext context)
        {
            _logger.LogInformation("call CartProto.GetCart");
             
            var cart = await GetCartEntity() ?? new Cart();

            return cart.ToResponseCartPhoto();
        }

        private async Task<Cart> GetCartEntity()
        { 
            return await _context.Carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.IdCustomer == _aspNetUser.GetUserId());
        } 
    }
}
