using Core.Security.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Security.Core;

public class JwksBuilder : IJwksBuilder
{

    public JwksBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public IServiceCollection Services { get; }
}