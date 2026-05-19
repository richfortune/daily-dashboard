using DailyDashboard.Application.DTOs;
using DailyDashboard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DailyDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    [HttpGet("rome")]
    public async Task<ActionResult<WeatherDto>> GetRomeWeather()
    {
        _logger.LogInformation("Inizio richiesta per ottenere i dati meteo di Roma");
        try
        {
            // Coordinates for Rome
            var weather = await _weatherService.GetCurrentWeatherAsync(41.9028, 12.4964);
            _logger.LogInformation("Dati meteo recuperati con successo per {City}. Temperatura: {Temperature}", "Roma", weather.Temperature);
            return Ok(weather);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei dati meteo di Roma");
            throw;
        }
    }
}
