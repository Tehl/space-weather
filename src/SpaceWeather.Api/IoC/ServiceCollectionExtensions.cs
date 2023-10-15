using SpaceWeather.Api.Repository;

namespace SpaceWeather.Api.IoC;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IMagneticIndexRepository, MagneticIndexRepository>();
    }
}
