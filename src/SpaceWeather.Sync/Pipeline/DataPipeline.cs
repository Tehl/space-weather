namespace SpaceWeather.Sync.Pipeline;

internal class DataPipeline<TSource, TModel> : IDataPipeline
{
    private readonly IDataSource<TSource> _source;
    private readonly IDataTransformer<TSource, TModel> _transformer;
    private readonly IDataRepository<TModel> _repository;

    public DataPipeline(
        IDataSource<TSource> source,
        IDataTransformer<TSource, TModel> transformer,
        IDataRepository<TModel> repository
    )
    {
        _source = source;
        _transformer = transformer;
        _repository = repository;
    }

    public async Task ExecuteAsync()
    {
        var data = await _source.GetData();
        var models = _transformer.Transform(data);
        await _repository.StoreAsync(models);
    }
}
