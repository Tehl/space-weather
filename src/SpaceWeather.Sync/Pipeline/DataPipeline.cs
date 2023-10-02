using Microsoft.Extensions.Logging;

namespace SpaceWeather.Sync.Pipeline;

internal class DataPipeline<TSource, TModel> : IDataPipeline
{
    private readonly IDataSource<TSource> _source;
    private readonly IDataTransformer<TSource, TModel> _transformer;
    private readonly IDataRepository<TModel> _repository;
    private readonly ILogger<DataPipeline<TSource, TModel>> _logger;

    public DataPipeline(
        IDataSource<TSource> source,
        IDataTransformer<TSource, TModel> transformer,
        IDataRepository<TModel> repository,
        ILogger<DataPipeline<TSource, TModel>> logger
    )
    {
        _source = source;
        _transformer = transformer;
        _repository = repository;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        try
        {
            _logger.LogInformation("Executing {model} data pipeline", typeof(TModel).Name);

            var data = await _source.GetData();

            _logger.LogInformation("Fetched data from {source}", _source.GetType().Name);

            var models = _transformer.Transform(data);

            _logger.LogInformation("Generated {count} data items using {transformer}", models.Length, _transformer.GetType().Name);

            await _repository.StoreAsync(models);

            _logger.LogInformation("Stored {count} data items using {repository}", models.Length, _repository.GetType().Name);

            _logger.LogInformation("Pipeline execution complete");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Pipeline failed with exception: {message}", ex.Message);
        }
    }
}
