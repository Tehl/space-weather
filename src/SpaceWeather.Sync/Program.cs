using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SpaceWeather.Domain.Context;
using SpaceWeather.Domain.IoC;
using SpaceWeather.Sync.Readers;
using SpaceWeather.Sync.SwpcApi;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        _ = services.AddHttpClient<ISwpcApiClient, SwpcApiClient>((s, client) =>
        {
            var options = s.GetRequiredService<IOptions<SwpcApiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseAddress);
        });

        _ = services.Configure<SwpcApiOptions>(context.Configuration.GetSection("SwpcApi"));

        _ = services.AddSpaceWeatherDbContext("SpaceWeather");
    })
    .Build();

var apiClient = host.Services.GetRequiredService<ISwpcApiClient>();

var forecastData = await apiClient.GetDailyForecastDataAsync();

var indices = new DailyForecastIndexReader().ReadIndices(forecastData);

using var scope = host.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<SpaceWeatherDbContext>();
