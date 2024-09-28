using Microsoft.EntityFrameworkCore;
using Store.ShopCart.API.Domain.Data.Contexts;

namespace Configurations
{
    public static class DbContextConfiguration
    {
        public static WebApplicationBuilder AddDbContextConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CartDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
             
            return builder;
        }
    }
}
