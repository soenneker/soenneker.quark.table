using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.Utils.ResourceLoader.Registrars;


namespace Soenneker.Quark;

/// <summary>
/// Service registrar for Quark.Table
/// </summary>
public static class QuarkTableRegistrar
{
    /// <summary>
    /// Adds QuarkTable services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddQuarkTable(this IServiceCollection services)
    {
        services.AddResourceLoaderAsScoped().TryAddScoped<IQuarkTableInterop, QuarkTableInterop>();
        return services;
    }
}
