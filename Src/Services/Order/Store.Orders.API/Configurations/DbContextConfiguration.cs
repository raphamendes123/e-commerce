using Microsoft.EntityFrameworkCore;
using Store.Orders.Infra.Data.Contexts;

namespace Configurations
{
    public static class DbContextConfiguration
    {
        public static WebApplicationBuilder AddDbContextConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<OrdersDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
             
            return builder;
        }
    }
}
