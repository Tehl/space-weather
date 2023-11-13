using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using SpaceWeather.Api.Repository;
using SpaceWeather.Domain.Models;

namespace SpaceWeather.Api.Controllers;

[Route("MagneticIndexReadings")]
public class MagneticIndexReadingController : Controller
{
    private readonly IMagneticIndexRepository _repository;
    private readonly ILogger<MagneticIndexReadingController> _logger;

    public MagneticIndexReadingController(
        IMagneticIndexRepository repository,
        ILogger<MagneticIndexReadingController> logger
    )
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    [Route("{station}/{type}")]
    [ProducesResponseType(typeof(MagneticIndexReading[]), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Search(
        [Required] MeasurementStation station,
        [Required] MagneticIndexType type,
        [Required][FromQuery] DateTimeOffset fromTimestamp,
        [Required][FromQuery] DateTimeOffset toTimestamp
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var results = await _repository.GetReadingsAsync(
            station,
            type,
            fromTimestamp,
            toTimestamp
        );

        _logger.LogInformation(
            "Got {count} {type}-index readings for station {station} between {fromTimestamp} and {toTimestamp}",
            results.Length,
            type,
            station,
            fromTimestamp,
            toTimestamp
        );

        return Ok(results);
    }
}
