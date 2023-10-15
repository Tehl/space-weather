using SpaceWeather.Domain.Models;

namespace SpaceWeather.Api.Repository;

public interface IMagneticIndexRepository
{
    Task<MagneticIndexReading[]> GetReadingsAsync(
        MeasurementStation station,
        MagneticIndexType type,
        DateTimeOffset fromTimestamp,
        DateTimeOffset toTimestamp
    );
}
