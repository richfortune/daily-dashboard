using DailyDashboard.Application.DTOs;
using DailyDashboard.Application.Interfaces;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DailyDashboard.Infrastructure.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
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
            return new WeatherDto { Temperature = "--", Description = "Condizioni non disponibili", LastUpdated = DateTime.Now.ToString("HH:mm") };
        }
    }

    public async Task<WeatherCurrentDto?> GetCurrentWeatherByCityAsync(string city)
    {
        var location = await GetLocationAsync(city);
        if (location == null) return null;

        try
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={location.Value.Latitude.ToString(CultureInfo.InvariantCulture)}&longitude={location.Value.Longitude.ToString(CultureInfo.InvariantCulture)}&current=temperature_2m,weather_code,wind_speed_10m&timezone=auto";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(jsonString);

            var currentElement = jsonDocument.RootElement.GetProperty("current");
            var temperature = currentElement.GetProperty("temperature_2m").GetDouble();
            var weatherCode = currentElement.GetProperty("weather_code").GetInt32();
            var windSpeed = currentElement.GetProperty("wind_speed_10m").GetDouble();

            return new WeatherCurrentDto
            {
                City = location.Value.Name,
                Country = location.Value.Country,
                Latitude = location.Value.Latitude,
                Longitude = location.Value.Longitude,
                Temperature = temperature,
                WeatherCode = weatherCode,
                WeatherDescription = GetWeatherDescription(weatherCode),
                WindSpeed = windSpeed,
                Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            };
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<WeatherTomorrowDto?> GetTomorrowWeatherByCityAsync(string city)
    {
        var location = await GetLocationAsync(city);
        if (location == null) return null;

        try
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={location.Value.Latitude.ToString(CultureInfo.InvariantCulture)}&longitude={location.Value.Longitude.ToString(CultureInfo.InvariantCulture)}&daily=temperature_2m_max,temperature_2m_min,weather_code,precipitation_probability_max&timezone=auto";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(jsonString);

            var dailyElement = jsonDocument.RootElement.GetProperty("daily");
            
            var timeArray = dailyElement.GetProperty("time");
            var tempMinArray = dailyElement.GetProperty("temperature_2m_min");
            var tempMaxArray = dailyElement.GetProperty("temperature_2m_max");
            var weatherCodeArray = dailyElement.GetProperty("weather_code");
            var precProbArray = dailyElement.GetProperty("precipitation_probability_max");

            if (timeArray.GetArrayLength() < 2) return null;

            var rawDate = timeArray[1].GetString() ?? string.Empty;
            string formattedDate = rawDate;
            if (DateTime.TryParseExact(rawDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                formattedDate = parsedDate.ToString("dd/MM/yyyy");
            }

            var tempMin = tempMinArray[1].GetDouble();
            var tempMax = tempMaxArray[1].GetDouble();
            var weatherCode = weatherCodeArray[1].GetInt32();
            var precProb = precProbArray[1].GetInt32();

            return new WeatherTomorrowDto
            {
                City = location.Value.Name,
                Country = location.Value.Country,
                Latitude = location.Value.Latitude,
                Longitude = location.Value.Longitude,
                Date = formattedDate,
                TemperatureMin = tempMin,
                TemperatureMax = tempMax,
                WeatherCode = weatherCode,
                WeatherDescription = GetWeatherDescription(weatherCode),
                PrecipitationProbability = precProb
            };
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async Task<(string Name, string Country, double Latitude, double Longitude)?> GetLocationAsync(string city)
    {
        try
        {
            var url = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(city)}&count=1&language=it&format=json";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(jsonString);

            if (!jsonDocument.RootElement.TryGetProperty("results", out var resultsElement) || resultsElement.GetArrayLength() == 0)
            {
                return null;
            }

            var firstResult = resultsElement[0];
            var name = firstResult.GetProperty("name").GetString() ?? city;
            var country = firstResult.TryGetProperty("country", out var countryProp) ? (countryProp.GetString() ?? string.Empty) : string.Empty;
            var latitude = firstResult.GetProperty("latitude").GetDouble();
            var longitude = firstResult.GetProperty("longitude").GetDouble();

            return (name, country, latitude, longitude);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private string GetWeatherDescription(int code)
    {
        return code switch
        {
            0 => "Sereno",
            1 or 2 or 3 => "Parzialmente nuvoloso",
            45 or 48 => "Nebbia",
            51 or 53 or 55 => "Pioviggine",
            61 or 63 or 65 => "Pioggia",
            80 or 81 or 82 => "Rovesci",
            95 => "Temporale",
            _ => "Condizioni non disponibili"
        };
    }
}
