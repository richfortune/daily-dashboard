using DailyDashboard.Application.DTOs;
using DailyDashboard.Application.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Globalization;

namespace DailyDashboard.Infrastructure.Services;

public class OpenMeteoWeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;

    public OpenMeteoWeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherDto> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        try
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude.ToString(CultureInfo.InvariantCulture)}&longitude={longitude.ToString(CultureInfo.InvariantCulture)}&current=temperature_2m,weather_code";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(jsonString);

            var currentElement = jsonDocument.RootElement.GetProperty("current");
            var temperature = currentElement.GetProperty("temperature_2m").GetDouble();
            var weatherCode = currentElement.GetProperty("weather_code").GetInt32();

            return new WeatherDto
            {
                Temperature = $"{Math.Round(temperature)}°C",
                Description = GetWeatherDescription(weatherCode),
                LastUpdated = DateTime.Now.ToString("HH:mm")
            };
        }
        catch (Exception)
        {
            return new WeatherDto { Temperature = "--", Description = "Weather unavailable", LastUpdated = DateTime.Now.ToString("HH:mm") };
        }
    }

    private string GetWeatherDescription(int code)
    {
        return code switch
        {
            0 => "Clear",
            1 or 2 or 3 => "Cloudy",
            45 or 48 => "Fog",
            51 or 53 or 55 => "Drizzle",
            61 or 63 or 65 => "Rain",
            71 or 73 or 75 => "Snow",
            80 or 81 or 82 => "Showers",
            95 => "Thunderstorm",
            _ => "Weather unavailable"
        };
    }
}
