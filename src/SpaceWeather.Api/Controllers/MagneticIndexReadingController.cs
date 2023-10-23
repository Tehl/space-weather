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

    public MagneticIndexReadingController(
        IMagneticIndexRepository repository
    )
    {
        _repository = repository;
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

        return Ok(results);
    }
}
