using DailyDashboard.Application.Interfaces;
using DailyDashboard.Infrastructure;
using DailyDashboard.Infrastructure.Persistence;
using DailyDashboard.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

var seqUrl = builder.Configuration["Serilog:SeqUrl"] ?? "http://localhost:5341";

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.Seq(seqUrl)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

// Register Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Register HttpClient and Services
builder.Services.AddHttpClient<IBitcoinService, CoinbaseBitcoinService>();
builder.Services.AddHttpClient<IWeatherService, OpenMeteoWeatherService>();

// Configure CORS for Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:3000",
                "https://niyama-dashboard.netlify.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Applica le migrazioni pendenti all'avvio dell'applicazione
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DailyDashboardDbContext>();
        Log.Information("Avvio delle migrazioni del database...");
        context.Database.Migrate();
        Log.Information("Migrazioni del database completate con successo.");
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Si è verificato un errore critico durante l'esecuzione delle migrazioni del database.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseSerilogRequestLogging();

app.UseCors("FrontendPolicy");

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.ToString(),
                description = e.Value.Description,
                error = e.Value.Exception?.Message
            })
        };

        await JsonSerializer.SerializeAsync(context.Response.Body, response);
    }
});

app.Run();
