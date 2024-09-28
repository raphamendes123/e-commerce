using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Store.Catalog.API.Domain.Data.Entitys;
using Store.Catalog.API.Domain.Data.Contexts;

namespace Store.Catalog.API.Configurations
{

    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

            if (env.IsDevelopment() || env.IsEnvironment("Docker"))
            {
                await context.Database.EnsureCreatedAsync();
                await EnsureSeedProducts(context);
            }
            //remover depois
            await context.Database.EnsureCreatedAsync();
            await EnsureSeedProducts(context);
        }

        private static async Task EnsureSeedProducts(CatalogDbContext context)
        {
            if (context.Products.Any())
                return;

            await context.Products.AddAsync(new ProductEntity() { Name = "T-Shirt Code Life Black", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 1.00M, DateAdded = DateTime.Now, Image = "camiseta2.jpg", Stock = 2 });
            await context.Products.AddAsync(new ProductEntity() { Name = "T-Shirt Rick And Morty", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 2.00M, DateAdded = DateTime.Now, Image = "caneca4.jpg", Stock = 3 });
            await context.Products.AddAsync(new ProductEntity() { Name = "T-Shirt Try Hard", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 3.00M, DateAdded = DateTime.Now, Image = "caneca4.jpg", Stock = 1 });
            await context.Products.AddAsync(new ProductEntity() { Name = "Mug No Coffee No Code", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca4.jpg", Stock = 24 });
            await context.Products.AddAsync(new ProductEntity() { Name = "T-Shirt Debugar Black", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 75.00M, DateAdded = DateTime.Now, Image = "camiseta4.jpg", Stock = 1 });
            await context.Products.AddAsync(new ProductEntity() { Name = "T-Shirt Code Life Gray", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 99.00M, DateAdded = DateTime.Now, Image = "camiseta3.jpg", Stock = 2 });
            await context.Products.AddAsync(new ProductEntity() { Name = "Mug Star Bugs Coffee", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 20.00M, DateAdded = DateTime.Now, Image = "caneca1.jpg", Stock = 2 });
            await context.Products.AddAsync(new ProductEntity() { Name = "Mug Programmer Code", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 15.00M, DateAdded = DateTime.Now, Image = "caneca2.jpg", Stock = 1 });
            await context.Products.AddAsync(new ProductEntity() { Name = "T-Shirt Software Developer", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 100.00M, DateAdded = DateTime.Now, Image = "camiseta1.jpg", Stock = 2 });
            await context.Products.AddAsync(new ProductEntity() { Name = "Mug Turn Coffee into Code", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 22.00M, DateAdded = DateTime.Now, Image = "caneca3.jpg", Stock = 1 });

            await context.SaveChangesAsync();
        }
    }

}
