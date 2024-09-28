using System;
using Core.Security.Core;
using Core.Security.Core.Interfaces;
using Core.Security.Jwt.Identity.Data;
using Core.Security.Jwt.Identity.Interfaces;
using Core.Security.Jwt.Identity.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
 

namespace Core.Security.Jwt.Identity.NovaPasta;

public static class JwtBuilderExtensions
{
    public static IJwksBuilder AddIdentity<TIdentityUser, TKey>(this IServiceCollection services, Action<JwtOptions> options = null)
        where TIdentityUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IJwtBuilder, JwtBuilderInject<TIdentityUser, TKey>>();
        return services.AddJwksManager(options);
    }

    public static IJwksBuilder AddIdentity<TIdentityUser>(this IServiceCollection services, Action<JwtOptions> options = null)
        where TIdentityUser : IdentityUser
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IJwtBuilder, JwtBuilderInject<TIdentityUser, string>>();
        return services.AddJwksManager(options);
    }

    public static IJwksBuilder AddIdentity(this IServiceCollection services, Action<JwtOptions> options = null)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IJwtBuilder, JwtBuilderInject<IdentityUser, string>>();
        return services.AddJwksManager(options);
    }

    public static IJwksBuilder AddIdentity<TIdentityUser, TKey>(this IJwksBuilder services)
        where TIdentityUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        services.Services.AddHttpContextAccessor();
        services.Services.AddScoped<IJwtBuilder, JwtBuilderInject<TIdentityUser, TKey>>();
        return services;
    }

    public static IJwksBuilder AddIdentity<TIdentityUser>(this IJwksBuilder services)
        where TIdentityUser : IdentityUser
    {
        services.Services.AddHttpContextAccessor();
        services.Services.AddScoped<IJwtBuilder, JwtBuilderInject<TIdentityUser, string>>();
        return services;
    }

    public static IJwksBuilder AddIdentity(this IJwksBuilder services)
    {
        services.Services.AddHttpContextAccessor();
        services.Services.AddScoped<IJwtBuilder, JwtBuilderInject<IdentityUser, string>>();
        return services;
    }

    public static IdentityBuilder AddIdentityConfiguration(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentException(nameof(services));

        return services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityAppDbContext>()
                .AddDefaultTokenProviders();
    }

    public static IdentityBuilder AddDefaultIdentity(this IServiceCollection services, Action<IdentityOptions> options = null)
    {
        if (services == null) throw new ArgumentException(nameof(services));
        return services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddDefaultTokenProviders();
    }

    public static IdentityBuilder AddCustomIdentity<TIdentityUser>(this IServiceCollection services, Action<IdentityOptions> options = null)
        where TIdentityUser : IdentityUser
    {
        if (services == null) throw new ArgumentException(nameof(services));

        return services.AddIdentity<TIdentityUser, IdentityRole>(options)
            .AddDefaultTokenProviders();
    }

    public static IdentityBuilder AddCustomIdentity<TIdentityUser, TKey>(this IServiceCollection services, Action<IdentityOptions> options = null)
        where TIdentityUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        if (services == null) throw new ArgumentException(nameof(services));
        return services.AddIdentity<TIdentityUser, IdentityRole<TKey>>(options)
            .AddDefaultTokenProviders();
    }

    public static IdentityBuilder AddDefaultRoles(this IdentityBuilder builder)
    {
        return builder.AddRoles<IdentityRole>();
    }

    public static IdentityBuilder AddCustomRoles<TRole>(this IdentityBuilder builder)
        where TRole : IdentityRole
    {
        return builder.AddRoles<TRole>();
    }

    public static IdentityBuilder AddCustomRoles<TRole, TKey>(this IdentityBuilder builder)
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        return builder.AddRoles<TRole>();
    }

    public static IdentityBuilder AddDefaultEntityFrameworkStores(this IdentityBuilder builder)
    {
        return builder.AddEntityFrameworkStores<IdentityAppDbContext>();
    }

    public static IdentityBuilder AddCustomEntityFrameworkStores<TContext>(this IdentityBuilder builder) where TContext : DbContext
    {
        return builder.AddEntityFrameworkStores<TContext>();
    }

    public static IServiceCollection AddIdentityEntityFrameworkContextConfiguration(
        this IServiceCollection services, Action<DbContextOptionsBuilder> options)
    {
        if (services == null) throw new ArgumentException(nameof(services));
        if (options == null) throw new ArgumentException(nameof(options));
        return services.AddDbContext<IdentityAppDbContext>(options);
    } 
}
