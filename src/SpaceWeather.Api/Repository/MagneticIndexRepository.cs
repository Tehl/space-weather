using Microsoft.EntityFrameworkCore;
using SpaceWeather.Domain.Context;
using SpaceWeather.Domain.Models;

namespace SpaceWeather.Api.Repository;

internal class MagneticIndexRepository : IMagneticIndexRepository
{
    private readonly SpaceWeatherDbContext _dbContext;

    public MagneticIndexRepository(
        SpaceWeatherDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    public Task<MagneticIndexReading[]> GetReadingsAsync(
        MeasurementStation station,
        MagneticIndexType type,
        DateTimeOffset fromTimestamp,
        DateTimeOffset toTimestamp
    )
    {
        return _dbContext.MagneticIndexReadings
            .Where(x =>
                x.StartTimeUtc >= fromTimestamp
                && x.EndTimeUtc <= toTimestamp
                && x.Station == station
                && x.Type == type
            )
            .OrderBy(x => x.StartTimeUtc)
            .ToArrayAsync();
    }
}
