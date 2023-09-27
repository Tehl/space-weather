using SpaceWeather.Domain.Models;
using SpaceWeather.Sync.Pipeline;

namespace SpaceWeather.Sync.Data;

internal class DailyRecordedIndexPipeline : DataPipeline<string, MagneticIndexReading>
{
    public DailyRecordedIndexPipeline(
        DailyRecordedIndexSource source,
        DailyRecordedIndexTransformer transformer,
        IDataRepository<MagneticIndexReading> repository
    ) : base(source, transformer, repository)
    {
    }
}
