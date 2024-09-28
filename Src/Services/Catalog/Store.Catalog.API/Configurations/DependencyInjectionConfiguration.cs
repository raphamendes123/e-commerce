using Store.Catalog.API.Domain.Data.Repositorys;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Core.ApiConfigurations;
using Store.Catalog.API.Domain.Data.Repositorys.Interfaces;
using Store.Catalog.API.Domain.Data.Contexts;

namespace Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<CatalogDbContext>();

            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IAspNetUser, AspNetUser>();

            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();


            return builder;
        }
    }
}
