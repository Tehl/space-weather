using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SpaceWeather.Domain.Models;
using SpaceWeather.Sync.Data;
using SpaceWeather.Sync.Pipeline;
using SpaceWeather.Sync.Repository;
using SpaceWeather.Sync.SwpcApi;

namespace SpaceWeather.Sync.IoC;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        _ = services.AddHttpClient<ISwpcApiClient, SwpcApiClient>((s, client) =>
        {
            var options = s.GetRequiredService<IOptions<SwpcApiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseAddress);
        });

        return services
            .AddScoped<DailyForecastIndexSource>()
            .AddScoped<DailyForecastIndexTransformer>()
            .AddScoped<IDataPipeline, DailyForecastIndexPipeline>()
            .AddScoped<IDataRepository<MagneticIndexReading>, MagneticIndexRepository>();
    }
}
