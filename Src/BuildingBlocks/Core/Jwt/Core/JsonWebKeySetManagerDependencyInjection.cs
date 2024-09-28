using Core.Security.Core.DefaultStore;
using Core.Security.Core.Interfaces;
using Core.Security.Core.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Security.Core;

public static class JsonWebKeySetManagerDependencyInjection
{
    /// <summary>
    /// Sets the signing credential.
    /// </summary>
    /// <returns></returns>
    public static IJwksBuilder AddJwksManager(this IServiceCollection services, Action<JwtOptions> action = null)
    {
        if (action != null)
            services.Configure(action);

        services.AddDataProtection();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IJsonWebKeyStore, DataProtectionStore>();

        return new JwksBuilder(services);
    }

    /// <summary>
    /// Sets the signing credential.
    /// </summary>
    /// <returns></returns>
    public static IJwksBuilder PersistKeysInMemory(this IJwksBuilder builder)
    {
        builder.Services.AddScoped<IJsonWebKeyStore, InMemoryStore>();

        return builder;
    }
}