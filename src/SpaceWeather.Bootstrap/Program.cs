using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpaceWeather.Domain.Context;
using SpaceWeather.Domain.IoC;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        _ = services.AddSpaceWeatherDbContext("SpaceWeather");
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<SpaceWeatherDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}
