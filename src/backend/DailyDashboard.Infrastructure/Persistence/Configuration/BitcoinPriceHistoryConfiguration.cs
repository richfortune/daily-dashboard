using DailyDashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyDashboard.Infrastructure.Persistence.Configurations;

public class BitcoinPriceHistoryConfiguration
    : IEntityTypeConfiguration<BitcoinPriceHistory>
{
    public void Configure(EntityTypeBuilder<BitcoinPriceHistory> builder)
    {
        builder.ToTable("BitcoinPriceHistories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Price)
            .HasPrecision(18, 2);

        builder.Property(x => x.Currency)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Source)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Timestamp)
            .IsRequired();
    }
}