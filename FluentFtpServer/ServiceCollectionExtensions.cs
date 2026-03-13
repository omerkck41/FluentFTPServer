using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentFtpServer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentFtpServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FtpOptions>(configuration.GetSection(FtpOptions.SectionName));
        services.AddScoped<IFtpClient, FluentFtpClient>();
        return services;
    }

    public static IServiceCollection AddFluentFtpServer(this IServiceCollection services, Action<FtpOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddScoped<IFtpClient, FluentFtpClient>();
        return services;
    }
}
