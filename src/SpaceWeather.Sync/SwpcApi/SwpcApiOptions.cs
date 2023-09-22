namespace SpaceWeather.Sync.SwpcApi;

internal class SwpcApiOptions
{
    public string BaseAddress { get; set; } = default!;
    public string DailyRecordedDataUri { get; set; } = default!;
    public string DailyForecastDataUri { get; set; } = default!;
}
