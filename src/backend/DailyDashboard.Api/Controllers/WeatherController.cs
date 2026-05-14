using DailyDashboard.Application.DTOs;
using DailyDashboard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DailyDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("rome")]
    public async Task<ActionResult<WeatherDto>> GetRomeWeather()
    {
        // Coordinates for Rome
        var weather = await _weatherService.GetCurrentWeatherAsync(41.9028, 12.4964);
        return Ok(weather);
    }
}
