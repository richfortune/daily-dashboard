using DailyDashboard.Application.Interfaces;
using DailyDashboard.Infrastructure.Persistence;
using DailyDashboard.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DailyDashboard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DailyDashboardDbContext>(options =>
            options.UseNpgsql(connectionString,
                b => b.MigrationsAssembly(typeof(DailyDashboardDbContext).Assembly.FullName)));

        services.AddScoped<IBitcoinPriceHistoryService, BitcoinPriceHistoryService>();

        return services;
    }
}
