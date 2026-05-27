using System.ComponentModel.DataAnnotations;

namespace DailyDashboard.Application.DTOs;

public class CreateWeatherFavoriteLocationRequest
{
    [Required(ErrorMessage = "Il nome della città è obbligatorio")]
    [MaxLength(200, ErrorMessage = "Il nome della città non può superare i 200 caratteri")]
    public string CityName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Il paese è obbligatorio")]
    [MaxLength(100, ErrorMessage = "Il nome del paese non può superare i 100 caratteri")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "La latitudine è obbligatoria")]
    public double Latitude { get; set; }

    [Required(ErrorMessage = "La longitudine è obbligatoria")]
    public double Longitude { get; set; }
}
