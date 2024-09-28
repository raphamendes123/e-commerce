using Core.Mediator;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Core.ApiConfigurations;
using Store.Payment.API.Domain.Data.Contexts;
using Store.Payment.API.Facade.Interfaces;
using Store.Payment.API.Facade;
using Store.Payment.API.Services.Interfaces;
using Store.Payment.API.Services;
using Store.Payment.API.Domain.Data.Repository.Interfaces;
using Store.Payment.API.Domain.Data.Repository;

namespace Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            builder.Services.AddScoped<IAspNetUser, AspNetUser>();
            builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();


            builder.Services.AddScoped<PaymentDbContext>();
            builder.Services.AddScoped<IPaymentFacade, CardCreditPaymentFacade>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

            return builder;
        }
    }
}
