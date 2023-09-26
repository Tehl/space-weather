using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpaceWeather.Domain.IoC;
using SpaceWeather.Sync.IoC;
using SpaceWeather.Sync.Pipeline;
using SpaceWeather.Sync.SwpcApi;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        _ = services.AddApplicationServices();
        _ = services.AddSpaceWeatherDbContext("SpaceWeather");

        _ = services.Configure<SwpcApiOptions>(context.Configuration.GetSection("SwpcApi"));
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    foreach (var pipeline in scope.ServiceProvider.GetServices<IDataPipeline>())
    {
        await pipeline.ExecuteAsync();
    }
}

