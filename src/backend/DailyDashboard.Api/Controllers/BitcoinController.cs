using DailyDashboard.Application.DTOs;
using DailyDashboard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DailyDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BitcoinController : ControllerBase
{
    private readonly IBitcoinService _bitcoinService;
    private readonly IBitcoinPriceHistoryService _bitcoinPriceHistoryService;
    private readonly IBitcoinPriceHistoryQueryService _bitcoinPriceHistoryQueryService;
    private readonly ILogger<BitcoinController> _logger;

    public BitcoinController(
        IBitcoinService bitcoinService,
        IBitcoinPriceHistoryService bitcoinPriceHistoryService,
        IBitcoinPriceHistoryQueryService bitcoinPriceHistoryQueryService,
        ILogger<BitcoinController> logger)
    {
        _bitcoinService = bitcoinService;
        _bitcoinPriceHistoryService = bitcoinPriceHistoryService;
        _bitcoinPriceHistoryQueryService = bitcoinPriceHistoryQueryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<BitcoinPriceDto>> Get()
    {
        _logger.LogInformation("Inizio richiesta per ottenere il prezzo corrente del Bitcoin");
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

    [HttpPost("history/snapshot")]
    public async Task<ActionResult<BitcoinPriceHistoryDto>> CreateSnapshot()
    {
        _logger.LogInformation("Inizio richiesta di salvataggio snapshot prezzo Bitcoin");
        try
        {
            var priceDto = await _bitcoinService.GetCurrentPriceAsync();
            _logger.LogInformation("Prezzo Bitcoin corrente recuperato per snapshot. Prezzo: {Price}", priceDto.Price);

            if (priceDto.Price == "--")
            {
                return BadRequest("Impossibile recuperare il prezzo corrente del Bitcoin da Coinbase");
            }

            var cleanPrice = priceDto.Price.Replace("$", "").Replace(",", "").Trim();
            if (!decimal.TryParse(cleanPrice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var decimalPrice))
            {
                _logger.LogWarning("Impossibile convertire il prezzo '{Price}' in decimale", priceDto.Price);
                return BadRequest($"Impossibile convertire il prezzo '{priceDto.Price}' in decimale");
            }

            var savedSnapshot = await _bitcoinPriceHistoryService.SavePriceHistoryAsync(decimalPrice, "USD", "Coinbase");
            _logger.LogInformation("Snapshot prezzo Bitcoin salvato nel database con successo. ID: {Id}", savedSnapshot.Id);

            return Ok(savedSnapshot);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione dello snapshot del prezzo del Bitcoin");
            throw;
        }
    }

    [HttpGet("history")]
    public async Task<ActionResult<List<BitcoinPriceHistoryDto>>> GetHistory()
    {
        _logger.LogInformation("Inizio richiesta per ottenere la cronologia dei prezzi Bitcoin");
        try
        {
            var history = await _bitcoinPriceHistoryQueryService.GetLatestAsync();
            return Ok(history);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero della cronologia dei prezzi Bitcoin");
            throw;
        }
    }
}
