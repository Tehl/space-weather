using SpaceWeather.Domain.Models;
using SpaceWeather.Sync.Pipeline;

namespace SpaceWeather.Sync.Data;

internal class DailyForecastIndexPipeline : DataPipeline<string, MagneticIndexReading>
{
    public DailyForecastIndexPipeline(
        DailyForecastIndexSource source,
        DailyForecastIndexTransformer transformer,
        IDataRepository<MagneticIndexReading> repository
    ) : base(source, transformer, repository)
    {
    }
}
