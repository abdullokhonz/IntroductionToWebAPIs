using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using IntroductionToWebAPIs.Extensions;
using IntroductionToWebAPIs.HealthChecks;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Validations;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//if (!builder.Environment.IsEnvironment("Testing"))
//{
//    builder.Services.AddDbContext<PostgreSQLDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DbPostgres"))
//    .LogTo(Console.Write, LogLevel.Information)
//    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
//}

builder.Services.AddDbContext<PostgreSQLDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbPostgres"))
    .LogTo(Console.Write, LogLevel.Information)
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Services DI
builder.Services.AddMyServices();

builder.Services.AddControllers();

// HealthChecks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddCheck("self", () => HealthCheckResult.Healthy("Application is running"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Автоматически создаём документ для каждой версии API
    var provider = builder.Services.BuildServiceProvider()
        .GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerDoc(
            description.GroupName,
            new OpenApiInfo
            {
                Title = $"IntroductionToWebAPIs",
                Version = description.GroupName,
                Description = description.IsDeprecated ? "Deprecated" : null
            });
    }
});

// ВЕРСИОНИРОВАНИЕ — современный способ 2024–2025
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";        // v1, v2, v1.1
    options.SubstituteApiVersionInUrl = true;
});

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

var app = builder.Build();

var env = app.Services.GetRequiredService<IWebHostEnvironment>();
string uploadsPath;

if (env.IsEnvironment("Testing")) // если тестовое окружение
{
    uploadsPath = Path.Combine(Path.GetTempPath(), "IntroductionToWebAPIs", "uploads");
}
else // для разработки/прода
{
    uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
}

// Validate AutoMapper after app build
app.ValidateAutoMapper();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        // Добавляем все версии в дропдаун (новые сверху)
        foreach (var description in provider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Health check endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            duration = report.TotalDuration.TotalMilliseconds,
            timestamp = DateTime.UtcNow,
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                duration = entry.Value.Duration.TotalMilliseconds,
                description = entry.Value.Description,
                data = entry.Value.Data
            })
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        }));
    }
});

app.MapHealthChecks("/health/simple");
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = check => check.Name == "self" });
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = check => check.Name == "database" });

app.MapControllers();

app.Run();

public partial class Program { }
