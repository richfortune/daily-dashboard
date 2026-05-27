using DailyDashboard.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyDashboard.Application.Interfaces;

public interface IWeatherFavoriteLocationService
{
    Task<List<WeatherFavoriteLocationDto>> GetAllAsync();
    Task<WeatherFavoriteLocationDto> CreateAsync(CreateWeatherFavoriteLocationRequest request);
    Task DeleteAsync(int id);
}
