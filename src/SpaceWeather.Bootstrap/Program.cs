using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpaceWeather.Domain.Context;
using SpaceWeather.Domain.IoC;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        _ = logging.ClearProviders().AddConsole();
    })
    .ConfigureServices((context, services) =>
    {
        _ = services.AddSpaceWeatherDbContext("SpaceWeather");
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Starting application bootstrap at {timestamp}", DateTimeOffset.UtcNow);

        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<SpaceWeatherDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        logger.LogInformation("Application bootstrap complete at {timestamp}", DateTimeOffset.UtcNow);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to bootstrap application: {message}", ex.Message);
    }
}
