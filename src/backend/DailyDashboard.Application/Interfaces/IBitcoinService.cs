using DailyDashboard.Application.DTOs;
using System.Threading.Tasks;

namespace DailyDashboard.Application.Interfaces;

public interface IBitcoinService
{
    Task<BitcoinPriceDto> GetCurrentPriceAsync();
}
