using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpaceWeather.Domain.IoC;
using SpaceWeather.Sync.IoC;
using SpaceWeather.Sync.Pipeline;
using SpaceWeather.Sync.SwpcApi;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        _ = logging.ClearProviders().AddConsole();
    })
    .ConfigureServices((context, services) =>
    {
        _ = services.AddApplicationServices();
        _ = services.AddSpaceWeatherDbContext("SpaceWeather");

        _ = services.Configure<SwpcApiOptions>(context.Configuration.GetSection("SwpcApi"));
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Starting sync at {timestamp}", DateTimeOffset.UtcNow);

        foreach (var pipeline in scope.ServiceProvider.GetServices<IDataPipeline>())
        {
            await pipeline.ExecuteAsync();
        }

        logger.LogInformation("Sync complete at {timestamp}", DateTimeOffset.UtcNow);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to execute sync: {message}", ex.Message);
    }
}

