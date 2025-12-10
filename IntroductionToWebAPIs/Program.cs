using IntroductionToWebAPIs.Extensions;
using IntroductionToWebAPIs.Infrastructure;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Validate AutoMapper after app build
app.ValidateAutoMapper();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
