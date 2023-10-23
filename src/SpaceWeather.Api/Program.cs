using System.Text.Json.Serialization;
using SpaceWeather.Api.IoC;
using SpaceWeather.Api.Routing;
using SpaceWeather.Domain.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddSpaceWeatherDbContext("SpaceWeather");

builder.Services
    .AddControllers(options =>
    {
        options.UseGeneralRoutePrefix("api");
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
