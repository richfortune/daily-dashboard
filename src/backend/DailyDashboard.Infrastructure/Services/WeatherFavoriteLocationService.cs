using DailyDashboard.Application.DTOs;
using DailyDashboard.Application.Interfaces;
using DailyDashboard.Domain.Entities;
using DailyDashboard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyDashboard.Infrastructure.Services;

public class WeatherFavoriteLocationService : IWeatherFavoriteLocationService
{
    private readonly DailyDashboardDbContext _dbContext;

    public WeatherFavoriteLocationService(DailyDashboardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<WeatherFavoriteLocationDto>> GetAllAsync()
    {
        return await _dbContext.WeatherFavoriteLocations
            .AsNoTracking()
            .OrderBy(x => x.CityName)
            .Select(x => new WeatherFavoriteLocationDto
            {
                Id = x.Id,
                CityName = x.CityName,
                Country = x.Country,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<WeatherFavoriteLocationDto> CreateAsync(CreateWeatherFavoriteLocationRequest request)
    {
        var entity = new WeatherFavoriteLocation
        {
            CityName = request.CityName,
            Country = request.Country,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.WeatherFavoriteLocations.Add(entity);
        await _dbContext.SaveChangesAsync();

        return new WeatherFavoriteLocationDto
        {
            Id = entity.Id,
            CityName = entity.CityName,
            Country = entity.Country,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude,
            CreatedAt = entity.CreatedAt
        };
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbContext.WeatherFavoriteLocations.FindAsync(id);
        if (entity != null)
        {
            _dbContext.WeatherFavoriteLocations.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
