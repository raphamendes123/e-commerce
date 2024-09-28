
using Core.ApiConfigurations;
using Front.MVC.Business.Authentication;
using Front.MVC.Business.Authentication.Interfaces;
using Front.MVC.Extensions;
using Front.MVC.Handlers;
using Front.MVC.Services.Authentication;
using Front.MVC.Services.Catalog;
using Front.MVC.Services.Checkout;
using Front.MVC.Services.Checkout.Interfaces;
using Front.MVC.Services.Customer;
using Front.MVC.Services.Customer.Interfaces;
using Front.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;


namespace Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static WebApplicationBuilder AddDependencyInjectionConfig(this WebApplicationBuilder builder)
        {
            //por cada instancia
            builder.Services.AddTransient<HttpClientAuthorizationDelegateHandler>();

            builder.Services.AddSingleton<IValidationAttributeAdapterProvider, ValidationAttributeAdapterProvider>();

            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IAspNetUser, AspNetUser>();
                     
            builder.Services.AddScoped<IAuthenticationBusiness, AuthenticationBusiness>();

            builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>()
                .AddPolicyHandler(PollyExtensions.WaitRetry())
                .AddHttpMessageHandler<HttpClientAuthorizationDelegateHandler>()
                //em algumas situacoes OK (SSL) somente para gRPC
                .AllowSelfSignedCertificate()
                .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))
                );

            builder.Services.AddHttpClient<ICatalogService, CatalogService>()
                .AddPolicyHandler(PollyExtensions.WaitRetry())
                .AddHttpMessageHandler<HttpClientAuthorizationDelegateHandler>()
                //em algumas situacoes OK (SSL) somente para gRPC
                .AllowSelfSignedCertificate()
                .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))
                );

            builder.Services.AddHttpClient<ICheckoutService, CheckoutService>()
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
