using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using IntroductionToWebAPIs.Extensions;
using IntroductionToWebAPIs.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PostgreSQLDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbPostgres"))
    .LogTo(Console.Write, LogLevel.Information)
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Services DI
builder.Services.AddMyServices();

builder.Services.AddControllers();
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
                Title = $"Restaurant API",
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

var app = builder.Build();

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

app.MapControllers();

app.Run();
