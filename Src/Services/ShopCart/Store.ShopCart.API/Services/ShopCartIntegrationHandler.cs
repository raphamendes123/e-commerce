using Core.Message.Integration.Background;
using MessageBus;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Store.ShopCart.API.Domain.Data.Contexts;

namespace Store.ShopCart.API.Services
{
    public class ShopCartIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public ShopCartIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.SubscribeAsync<OrderAuthorizedIntegrationEvent>("OrderAuthorized", RemoveShopCart);
        }

        private async Task RemoveShopCart(OrderAuthorizedIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CartDbContext>();

            var cart = await context.Carts
                .FirstOrDefaultAsync(c => c.IdCustomer == message.IdCustomer);

            if (cart != null)
            {
                context.Carts.Remove(cart);
                await context.SaveChangesAsync();
            }
        }
    }
}
