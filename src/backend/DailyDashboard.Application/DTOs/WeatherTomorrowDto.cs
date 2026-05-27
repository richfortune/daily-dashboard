namespace DailyDashboard.Application.DTOs;

public class WeatherTomorrowDto
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Date { get; set; } = string.Empty;
    public double TemperatureMin { get; set; }
    public double TemperatureMax { get; set; }
    public int WeatherCode { get; set; }
    public string WeatherDescription { get; set; } = string.Empty;
    public int PrecipitationProbability { get; set; }
}
