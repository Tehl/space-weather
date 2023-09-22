using Microsoft.Extensions.Options;

namespace SpaceWeather.Sync.SwpcApi;

internal class SwpcApiClient : ISwpcApiClient
{
    private readonly SwpcApiOptions _options;
    private readonly HttpClient _httpClient;

    public SwpcApiClient(
        HttpClient httpClient,
        IOptions<SwpcApiOptions> options
    )
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> GetDailyRecordedDataAsync()
    {
        var response = await _httpClient.GetAsync(_options.DailyRecordedDataUri);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetDailyForecastDataAsync()
    {
        var response = await _httpClient.GetAsync(_options.DailyForecastDataUri);
        return await response.Content.ReadAsStringAsync();
    }
}
