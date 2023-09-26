using SpaceWeather.Sync.Pipeline;
using SpaceWeather.Sync.SwpcApi;

namespace SpaceWeather.Sync.Data;

internal class DailyForecastIndexSource : IDataSource<string>
{
    private readonly ISwpcApiClient _swpcApiClient;

    public DailyForecastIndexSource(
        ISwpcApiClient swpcApiClient
    )
    {
        _swpcApiClient = swpcApiClient;
    }

    public Task<string> GetData()
    {
        return _swpcApiClient.GetDailyForecastDataAsync();
    }
}
