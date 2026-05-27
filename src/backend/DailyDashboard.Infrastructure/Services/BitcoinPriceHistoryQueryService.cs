using DailyDashboard.Application.DTOs;
using DailyDashboard.Application.Interfaces;
using DailyDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyDashboard.Infrastructure.Services;

public class BitcoinPriceHistoryQueryService : IBitcoinPriceHistoryQueryService
{
    private readonly DailyDashboardDbContext _dbContext;

    public BitcoinPriceHistoryQueryService(DailyDashboardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<BitcoinPriceHistoryDto>> GetLatestAsync(int maxRecords = 100)
    {
        return await _dbContext.BitcoinPriceHistories
            .AsNoTracking()
            .OrderByDescending(x => x.Timestamp)
            .Take(maxRecords)
            .Select(x => new BitcoinPriceHistoryDto
            {
                Id = x.Id,
                Price = x.Price,
                Currency = x.Currency,
                Source = x.Source,
                Timestamp = x.Timestamp
            })
            .ToListAsync();
    }
}
