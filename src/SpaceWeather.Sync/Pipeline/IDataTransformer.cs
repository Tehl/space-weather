namespace SpaceWeather.Sync.Pipeline;

internal interface IDataTransformer<TFrom, TTo>
{
    TTo[] Transform(TFrom rawData);
}
