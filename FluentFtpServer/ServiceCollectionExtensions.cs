using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentFtpServer;

/// <summary>
/// Extension methods for setting up FluentFtpServer services in an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds FluentFtpServer services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configuration">The configuration instance to bind <see cref="FtpOptions"/> from.</param>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddFluentFtpServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FtpOptions>(configuration.GetSection(FtpOptions.SectionName));
        services.AddScoped<IFtpClient, FluentFtpClient>();
        return services;
    }

    /// <summary>
    /// Adds FluentFtpServer services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configureOptions">An <see cref="Action{FtpOptions}"/> to configure the provided <see cref="FtpOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddFluentFtpServer(this IServiceCollection services, Action<FtpOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddScoped<IFtpClient, FluentFtpClient>();
        return services;
    }
}
