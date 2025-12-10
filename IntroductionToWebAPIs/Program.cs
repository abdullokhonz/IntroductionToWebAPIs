using AutoMapper;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Services.Service;
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var provider2 = builder.Services.BuildServiceProvider();
var mapper = provider2.GetRequiredService<IMapper>();

mapper.ConfigurationProvider.AssertConfigurationIsValid();

try
{
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
    Console.WriteLine(" AutoMapper configuration is valid");
}
catch (AutoMapper.AutoMapperConfigurationException ex)
{
    Console.WriteLine(" AutoMapper configuration error:");
    Console.WriteLine(ex.Message);
    if (ex.InnerException != null)
        Console.WriteLine(ex.InnerException.Message);
    throw;
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
