namespace SpaceWeather.Sync.Pipeline;

internal interface IDataPipeline
{
    Task ExecuteAsync();
}