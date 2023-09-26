namespace SpaceWeather.Sync.Pipeline;

internal interface IDataRepository<TModel>
{
    Task StoreAsync(TModel[] models);
}
