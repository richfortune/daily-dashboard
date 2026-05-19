using DailyDashboard.Application.DTOs;
using DailyDashboard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DailyDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BitcoinController : ControllerBase
{
    private readonly IBitcoinService _bitcoinService;
    private readonly ILogger<BitcoinController> _logger;

    public BitcoinController(IBitcoinService bitcoinService, ILogger<BitcoinController> logger)
    {
        _bitcoinService = bitcoinService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<BitcoinPriceDto>> Get()
    {
        _logger.LogInformation("Inizio richiesta per ottenere il prezzo del Bitcoin");
        try
        {
            var price = await _bitcoinService.GetCurrentPriceAsync();
            _logger.LogInformation("Prezzo Bitcoin recuperato con successo. Prezzo: {Price}", price.Price);
            return Ok(price);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero del prezzo del Bitcoin");
            throw;
        }
    }
}
