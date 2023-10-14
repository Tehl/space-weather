using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceWeather.Domain.Context;

namespace SpaceWeather.Api.Controllers;

[Route("MagneticIndexReadings")]
public class MagneticIndexReadingController : Controller
{
    private readonly SpaceWeatherDbContext _dbContext;

    public MagneticIndexReadingController(
        SpaceWeatherDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var results = await _dbContext.MagneticIndexReadings.ToListAsync();
        return Ok(results);
    }
}
