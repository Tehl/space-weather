using SpaceWeather.Sync.Pipeline;
using SpaceWeather.Sync.SwpcApi;

namespace SpaceWeather.Sync.Data;

internal class DailyRecordedIndexSource : IDataSource<string>
{
    private readonly ISwpcApiClient _swpcApiClient;

    public DailyRecordedIndexSource(
        ISwpcApiClient swpcApiClient
    )
    {
        _swpcApiClient = swpcApiClient;
    }

    public Task<string> GetData()
    {
        return _swpcApiClient.GetDailyRecordedDataAsync();
    }
}
