using DailyDashboard.Application.DTOs;
using DailyDashboard.Application.Interfaces;
using DailyDashboard.Domain.Entities;
using DailyDashboard.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace DailyDashboard.Infrastructure.Services;

public class BitcoinPriceHistoryService : IBitcoinPriceHistoryService
{
    private readonly DailyDashboardDbContext _dbContext;

    public BitcoinPriceHistoryService(DailyDashboardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BitcoinPriceHistoryDto> SavePriceHistoryAsync(decimal price, string currency, string source)
    {
        var history = new BitcoinPriceHistory
        {
            Price = price,
            Currency = currency,
            Source = source,
            Timestamp = DateTime.UtcNow
        };

        _dbContext.BitcoinPriceHistories.Add(history);
        await _dbContext.SaveChangesAsync();

        return new BitcoinPriceHistoryDto
        {
            Id = history.Id,
            Price = history.Price,
            Currency = history.Currency,
            Source = history.Source,
            Timestamp = history.Timestamp
        };
    }
}
