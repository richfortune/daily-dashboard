using System.Threading.Tasks;

namespace DailyDashboard.Application.Interfaces;

public interface IBitcoinPriceHistoryService
{
    Task SavePriceHistoryAsync(decimal price, string currency, string source);
}
