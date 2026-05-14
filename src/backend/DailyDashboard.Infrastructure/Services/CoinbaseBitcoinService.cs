using DailyDashboard.Application.DTOs;
using DailyDashboard.Application.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Globalization;

namespace DailyDashboard.Infrastructure.Services;

public class CoinbaseBitcoinService : IBitcoinService
{
    private readonly HttpClient _httpClient;

    public CoinbaseBitcoinService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BitcoinPriceDto> GetCurrentPriceAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://api.coinbase.com/v2/prices/BTC-USD/spot");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(jsonString);
            
            var amountString = jsonDocument.RootElement
                .GetProperty("data")
                .GetProperty("amount")
                .GetString();

            if (decimal.TryParse(amountString, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
            {
                return new BitcoinPriceDto
                {
                    Price = $"${amount.ToString("N0", CultureInfo.InvariantCulture)}",
                    LastUpdated = DateTime.Now.ToString("HH:mm")
                };
            }

            return new BitcoinPriceDto { Price = "--", LastUpdated = DateTime.Now.ToString("HH:mm") };
        }
        catch (Exception)
        {
            return new BitcoinPriceDto { Price = "--", LastUpdated = DateTime.Now.ToString("HH:mm") };
            // Log exception here in a real app
        }
    }
}
