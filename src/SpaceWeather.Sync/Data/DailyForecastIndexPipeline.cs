using Microsoft.Extensions.Logging;
using SpaceWeather.Domain.Models;
using SpaceWeather.Sync.Pipeline;

namespace SpaceWeather.Sync.Data;

internal class DailyForecastIndexPipeline : DataPipeline<string, MagneticIndexReading>
{
    public DailyForecastIndexPipeline(
        DailyForecastIndexSource source,
        DailyForecastIndexTransformer transformer,
        IDataRepository<MagneticIndexReading> repository,
        ILogger<DataPipeline<string, MagneticIndexReading>> logger
    ) : base(source, transformer, repository, logger)
    {
    }
}
