using DailyDashboard.Application.DTOs;
using System.Threading.Tasks;

namespace DailyDashboard.Application.Interfaces;

public interface IBitcoinPriceHistoryService
{
    Task<BitcoinPriceHistoryDto> SavePriceHistoryAsync(decimal price, string currency, string source);
}
