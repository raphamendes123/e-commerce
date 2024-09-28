using Core.Mediator;
using Store.Customer.API.Application.Commands;
using Store.Customer.API.Application.Commands.RegisterStore.Customer.Integration;
using Store.Customer.API.Application.Events;
using Store.Customer.API.Domain.Data.Repository;
using Store.Customer.API.Domain.Data.Repository.Interfaces;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Core.ApiConfigurations;
using Store.Customer.API.Domain.Data.Contexts;

namespace Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<CustomerDbContext>();
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            builder.Services.AddScoped<IAspNetUser, AspNetUser>();
            builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();


            builder.Services.AddScoped<IRequestHandler<RegisterCustomerCommand, ValidationResult>, RegisterCustomerHandler>();
            builder.Services.AddScoped<IRequestHandler<RegisterAddressCommand, ValidationResult>, RegisterAddressHandler>();
            builder.Services.AddScoped<IRequestHandler<UpdateAddressCommand, ValidationResult>, UpdateAddressHandler>();

            builder.Services.AddScoped<INotificationHandler<RegisterCustomerEvent>, RegisterCustomerEventHandler>();
             

            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

            return builder;
        }
    }
}
