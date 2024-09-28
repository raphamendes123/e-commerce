using Core.Mediator;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Core.ApiConfigurations;
using Store.Bff.Checkout.Handlers;
using Polly;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Store.Bff.Checkout.API.Services.Rest.Catalog;
using Store.Bff.Checkout.API.Services.Rest.Orders;
using Store.Bff.Checkout.API.Services.Rest.ShopCart;
using Store.Bff.Checkout.API.Services.Rest.Customer;
using Store.Bff.Checkout.API.Services.Rest.Catalog.Interfaces;
using Store.Bff.Checkout.API.Services.Rest.Orders.Interfaces;
using Store.Bff.Checkout.API.Services.Rest.ShopCart.Interfaces;
using Store.Bff.Checkout.API.Services.Rest.Customer.Interfaces;

namespace Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {//por cada instancia
            builder.Services.AddTransient<HttpClientAuthorizationDelegateHandler>();
            builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
            builder.Services.AddSingleton<IValidationAttributeAdapterProvider, ValidationAttributeAdapterProvider>();
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
            builder.Services.AddScoped<IAspNetUser, AspNetUser>();
             
            builder.Services.AddHttpClient<ICatalogService, CatalogService>()
                .AddPolicyHandler(PollyExtensions.WaitRetry())
                .AddHttpMessageHandler<HttpClientAuthorizationDelegateHandler>()
                //em algumas situacoes OK (SSL) somente para gRPC
                .AllowSelfSignedCertificate()
                .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))
                );

            builder.Services.AddHttpClient<IShopCartService, ShopCartService>()
                .AddPolicyHandler(PollyExtensions.WaitRetry())
                .AddHttpMessageHandler<HttpClientAuthorizationDelegateHandler>()
                //em algumas situacoes OK (SSL) somente para gRPC
                .AllowSelfSignedCertificate()
                .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))
                );

            builder.Services.AddHttpClient<IOrderService, OrderService>()
                .AddPolicyHandler(PollyExtensions.WaitRetry())
                .AddHttpMessageHandler<HttpClientAuthorizationDelegateHandler>()
                //em algumas situacoes OK (SSL) somente para gRPC
                .AllowSelfSignedCertificate()
                .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))
                );

            builder.Services.AddHttpClient<ICustomerService, CustomerService>()
                .AddPolicyHandler(PollyExtensions.WaitRetry())
                .AddHttpMessageHandler<HttpClientAuthorizationDelegateHandler>()
                //em algumas situacoes OK (SSL) somente para gRPC
                .AllowSelfSignedCertificate()
                .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))
                );

            return builder;
        }
    }
}
