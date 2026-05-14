namespace DailyDashboard.Application.DTOs;

public class WeatherDto
{
    public string Temperature { get; set; } = "--";
    public string Description { get; set; } = "--";
    public string LastUpdated { get; set; } = string.Empty;
}
