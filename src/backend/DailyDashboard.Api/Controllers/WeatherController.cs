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

    [HttpGet("current")]
    public async Task<ActionResult<WeatherCurrentDto>> GetCurrentWeather([FromQuery] string city)
    {
        _logger.LogInformation("Inizio richiesta per ottenere il meteo corrente per la città: {City}", city);
        if (string.IsNullOrWhiteSpace(city))
        {
            return BadRequest("Il parametro 'city' è obbligatorio");
        }

        try
        {
            var weather = await _weatherService.GetCurrentWeatherByCityAsync(city);
            if (weather == null)
            {
                _logger.LogWarning("Meteo corrente non trovato per la città: {City}", city);
                return NotFound($"Località non trovata: {city}");
            }

            _logger.LogInformation("Meteo corrente recuperato con successo per {City}. Temp: {Temp}", city, weather.Temperature);
            return Ok(weather);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero del meteo corrente per {City}", city);
            throw;
        }
    }

    [HttpGet("tomorrow")]
    public async Task<ActionResult<WeatherTomorrowDto>> GetTomorrowWeather([FromQuery] string city)
    {
        _logger.LogInformation("Inizio richiesta per ottenere il meteo di domani per la città: {City}", city);
        if (string.IsNullOrWhiteSpace(city))
        {
            return BadRequest("Il parametro 'city' è obbligatorio");
        }

        try
        {
            var weather = await _weatherService.GetTomorrowWeatherByCityAsync(city);
            if (weather == null)
            {
                _logger.LogWarning("Meteo di domani non trovato per la città: {City}", city);
                return NotFound($"Località non trovata: {city}");
            }

            _logger.LogInformation("Meteo di domani recuperato con successo per {City}.", city);
            return Ok(weather);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero del meteo di domani per {City}", city);
            throw;
        }
    }
}
