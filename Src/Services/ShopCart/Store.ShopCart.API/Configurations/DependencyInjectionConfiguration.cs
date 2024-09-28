using Core.Mediator;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Store.ShopCart.API.Business;
using Store.ShopCart.API.Business.Interfaces;
using Core.ApiConfigurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Store.ShopCart.API.Domain.Data.Contexts;

namespace Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<CartDbContext>();
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IAspNetUser, AspNetUser>(); 
                         
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            builder.Services.AddScoped<ICartBusiness, CartBusiness>();

            return builder;
        }
    }
}
