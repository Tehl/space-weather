namespace SpaceWeather.Sync.Pipeline;

internal interface IDataSource<TSource>
{
    Task<TSource> GetData();
}
