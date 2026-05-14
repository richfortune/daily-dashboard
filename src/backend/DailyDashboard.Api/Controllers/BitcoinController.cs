using DailyDashboard.Application.DTOs;
using DailyDashboard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DailyDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BitcoinController : ControllerBase
{
    private readonly IBitcoinService _bitcoinService;

    public BitcoinController(IBitcoinService bitcoinService)
    {
        _bitcoinService = bitcoinService;
    }

    [HttpGet]
    public async Task<ActionResult<BitcoinPriceDto>> Get()
    {
        var price = await _bitcoinService.GetCurrentPriceAsync();
        return Ok(price);
    }
}
