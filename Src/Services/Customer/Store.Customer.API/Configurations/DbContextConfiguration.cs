using Microsoft.EntityFrameworkCore;
using Store.Customer.API.Domain.Data.Contexts;

namespace Configurations
{
    public static class DbContextConfiguration
    {
        public static WebApplicationBuilder AddDbContextConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<CustomerDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
             
            return builder;
        }
    }
}
