using Microsoft.Extensions.DependencyInjection;

namespace Core.Security.Core.Interfaces;

public interface IJwksBuilder
{
    IServiceCollection Services { get; }
}