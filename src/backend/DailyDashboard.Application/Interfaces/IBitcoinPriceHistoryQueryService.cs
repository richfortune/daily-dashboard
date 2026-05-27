using DailyDashboard.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyDashboard.Application.Interfaces;

public interface IBitcoinPriceHistoryQueryService
{
    Task<List<BitcoinPriceHistoryDto>> GetLatestAsync(int maxRecords = 100);
}
