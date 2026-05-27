using DailyDashboard.Application.DTOs;
using System.Threading.Tasks;

namespace DailyDashboard.Application.Interfaces;

public interface IWeatherService
{
    Task<WeatherDto> GetCurrentWeatherAsync(double latitude, double longitude);
    Task<WeatherCurrentDto?> GetCurrentWeatherByCityAsync(string city);
    Task<WeatherTomorrowDto?> GetTomorrowWeatherByCityAsync(string city);
}
