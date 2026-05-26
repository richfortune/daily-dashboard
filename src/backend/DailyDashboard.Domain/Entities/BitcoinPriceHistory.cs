namespace DailyDashboard.Domain.Entities;

public class BitcoinPriceHistory
{
    public int Id { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = string.Empty;

    public string Source { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }
}