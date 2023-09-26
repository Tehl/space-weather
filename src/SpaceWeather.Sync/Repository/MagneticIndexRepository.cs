using Microsoft.EntityFrameworkCore;
using SpaceWeather.Domain.Context;
using SpaceWeather.Domain.Models;
using SpaceWeather.Sync.Pipeline;

namespace SpaceWeather.Sync.Repository;

internal class MagneticIndexRepository : IDataRepository<MagneticIndexReading>
{
    private readonly SpaceWeatherDbContext _dbContext;

    public MagneticIndexRepository(
        SpaceWeatherDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    public Task StoreAsync(MagneticIndexReading[] readings)
    {
        return _dbContext.MagneticIndexReadings
            .UpsertRange(readings)
            .On(x => new { x.StartTimeUtc, x.Type, x.Station })
            .WhenMatched((dbRecord, localRecord) => new MagneticIndexReading
            {
                EndTimeUtc = localRecord.EndTimeUtc,
                Value = localRecord.Value
            })
            .RunAsync();
    }
}
