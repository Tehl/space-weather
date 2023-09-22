namespace SpaceWeather.Sync.SwpcApi;

internal interface ISwpcApiClient
{
    Task<string> GetDailyForecastDataAsync();
    Task<string> GetDailyRecordedDataAsync();
}