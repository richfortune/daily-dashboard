using DailyDashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyDashboard.Infrastructure.Persistence.Configurations;

public class WeatherFavoriteLocationConfiguration : IEntityTypeConfiguration<WeatherFavoriteLocation>
{
    public void Configure(EntityTypeBuilder<WeatherFavoriteLocation> builder)
    {
        builder.ToTable("WeatherFavoriteLocations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CityName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Country)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Latitude)
            .IsRequired();

        builder.Property(x => x.Longitude)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}
