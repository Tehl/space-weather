using Microsoft.Extensions.Logging;
using SpaceWeather.Domain.Models;
using SpaceWeather.Sync.Pipeline;

namespace SpaceWeather.Sync.Data;

internal class DailyRecordedIndexPipeline : DataPipeline<string, MagneticIndexReading>
{
    public DailyRecordedIndexPipeline(
        DailyRecordedIndexSource source,
        DailyRecordedIndexTransformer transformer,
        IDataRepository<MagneticIndexReading> repository,
        ILogger<DataPipeline<string, MagneticIndexReading>> logger
    ) : base(source, transformer, repository, logger)
    {
    }
}
