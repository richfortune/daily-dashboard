using System;

namespace DailyDashboard.Domain.Entities;

public class WeatherFavoriteLocation
{
    public int Id { get; set; }
    public string CityName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CreatedAt { get; set; }
}
