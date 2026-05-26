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
    private readonly IBitcoinPriceHistoryService _bitcoinPriceHistoryService;
    private readonly ILogger<BitcoinController> _logger;

    public BitcoinController(
        IBitcoinService bitcoinService,
        IBitcoinPriceHistoryService bitcoinPriceHistoryService,
        ILogger<BitcoinController> logger)
    {
        _bitcoinService = bitcoinService;
        _bitcoinPriceHistoryService = bitcoinPriceHistoryService;
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

            if (price.Price != "--")
            {
                var cleanPrice = price.Price.Replace("$", "").Replace(",", "").Trim();
                if (decimal.TryParse(cleanPrice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var decimalPrice))
                {
                    await _bitcoinPriceHistoryService.SavePriceHistoryAsync(decimalPrice, "USD", "Coinbase");
                    _logger.LogInformation("Snapshot prezzo Bitcoin salvato nel database con successo");
                }
                else
                {
                    _logger.LogWarning("Impossibile convertire il prezzo '{Price}' in decimale", price.Price);
                }
            }

            return Ok(price);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero o salvataggio del prezzo del Bitcoin");
            throw;
        }
    }
}
