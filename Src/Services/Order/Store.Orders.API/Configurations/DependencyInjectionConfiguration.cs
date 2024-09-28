using Core.Mediator;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Core.ApiConfigurations;
using Polly;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Store.Orders.API.Application.Queries.Interface;
using Store.Orders.API.Application.Queries;
using Store.Orders.Infra.Data.Repositorys;
using Store.Orders.Infra.Data.Contexts;
using Store.Orders.Infra.Data.Repositorys.Interfaces;
using Store.Orders.API.Application.Commands.RegisterOrder;
using Store.Orders.API.Application.Events;

namespace Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            //API
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IAspNetUser, AspNetUser>();
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            //Commands
            builder.Services.AddScoped<IRequestHandler<RegisterOrderCommand, ValidationResult>, RegisterOrderHandler>();

            //Events
            builder.Services.AddScoped<INotificationHandler<OrderAuthorizedEvent>, OrderEventHandler>();


            //Application
            builder.Services.AddScoped<IOrderQuerie, OrderQuerie>();
            builder.Services.AddScoped<IVoucherQuerie, VoucherQuerie>();
            builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();

            //Data
            builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            builder.Services.AddScoped<OrdersDbContext>();
            return builder;
        }
    }
}
