using Store.Authentication.API.Services;
using Store.Authentication.API.Services.Interfaces;
using Core.ApiConfigurations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Store.Authentication.API.Domain.Data.Contexts; 
using Microsoft.AspNetCore.Identity; 

namespace Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ApplicationDbContext>();

            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IAspNetUser, AspNetUser>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return builder;
        }
    }
}
