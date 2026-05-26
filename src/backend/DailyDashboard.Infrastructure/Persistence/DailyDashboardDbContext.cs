using DailyDashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DailyDashboard.Infrastructure.Persistence;

public class DailyDashboardDbContext : DbContext
{
    public DailyDashboardDbContext(DbContextOptions<DailyDashboardDbContext> options)
        : base(options)
    {
    }

    public DbSet<BitcoinPriceHistory> BitcoinPriceHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DailyDashboardDbContext).Assembly);
    }
}
