using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaceWeather.Domain.Context;

namespace SpaceWeather.Domain.IoC;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSpaceWeatherDbContext(
        this IServiceCollection services,
        string connectionStringName
    )
    {
        return services.AddDbContext<SpaceWeatherDbContext>((serviceProvider, dbContextOptions) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(connectionStringName);

            var serverVersion = ServerVersion.AutoDetect(connectionString);

            _ = dbContextOptions.UseMySql(
                connectionString,
                serverVersion,
                x => x.MigrationsAssembly("SpaceWeather.Bootstrap")
            );
        });
    }
}
