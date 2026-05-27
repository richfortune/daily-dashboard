using System;

namespace DailyDashboard.Application.DTOs;

public class BitcoinPriceHistoryDto
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
