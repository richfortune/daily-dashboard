using DailyDashboard.Application.DTOs;
using DailyDashboard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DailyDashboard.Api.Controllers;

[ApiController]
[Route("api/Weather/favorites")]
public class WeatherFavoritesController : ControllerBase
{
    private readonly IWeatherFavoriteLocationService _favoritesService;
    private readonly ILogger<WeatherFavoritesController> _logger;

    public WeatherFavoritesController(
        IWeatherFavoriteLocationService favoritesService,
        ILogger<WeatherFavoritesController> logger)
    {
        _favoritesService = favoritesService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<WeatherFavoriteLocationDto>>> Get()
    {
        _logger.LogInformation("Inizio richiesta per ottenere le località meteo preferite");
        try
        {
            var favorites = await _favoritesService.GetAllAsync();
            return Ok(favorites);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero delle località meteo preferite");
            throw;
        }
    }

    [HttpPost]
    public async Task<ActionResult<WeatherFavoriteLocationDto>> Post([FromBody] CreateWeatherFavoriteLocationRequest request)
    {
        _logger.LogInformation("Inizio richiesta per aggiungere una località meteo preferita: {CityName}", request.CityName);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _favoritesService.CreateAsync(request);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'aggiunta della località preferita: {CityName}", request.CityName);
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("Inizio richiesta per eliminare la località meteo preferita con ID: {Id}", id);
        try
        {
            await _favoritesService.DeleteAsync(id);
            return NoContent();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione della località preferita con ID: {Id}", id);
            throw;
        }
    }
}
