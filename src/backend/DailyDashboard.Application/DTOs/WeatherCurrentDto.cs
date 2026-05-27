using System;

namespace DailyDashboard.Application.DTOs;

public class WeatherCurrentDto
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Temperature { get; set; }
    public int WeatherCode { get; set; }
    public string WeatherDescription { get; set; } = string.Empty;
    public double WindSpeed { get; set; }
    public string Timestamp { get; set; } = string.Empty;
}
