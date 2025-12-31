using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;
using Xunit.Abstractions;

namespace IntroductionToWebAPIs.Tests
{
    public class UnitsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly ITestOutputHelper _output;

        public UnitsControllerIntegrationTests(CustomWebApplicationFactory factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public async Task GetById_ConcurrentRequests_ReturnsSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            int concurrentRequests = 50;
            var tasks = new Task<HttpResponseMessage>[concurrentRequests];

            // Act
            for (int i = 0; i < concurrentRequests; i++)
            {
                tasks[i] = client.GetAsync("/api/Units/GetByIdv2");
            }

            await Task.WhenAll(tasks);

            // Assert
            foreach (var task in tasks)
            {
                var response = await task;
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();

                _output.WriteLine($"Response content: {content}");

                var unitsDtos = JsonSerializer.Deserialize<List<object>>(content);

                Assert.NotNull(unitsDtos);
                Assert.NotEmpty(unitsDtos);
                _output.WriteLine($"Number of units returned: {unitsDtos.Count}");
            }
        }
    }

    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                // Удаляем реальный DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<PostgreSQLDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Добавляем In-Memory Database
                services.AddDbContext<PostgreSQLDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestDb");
                });

                // Настраиваем AutoMapper
                services.AddAutoMapper(typeof(Program));

                // Заполняем тестовые данные
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<PostgreSQLDbContext>();

                // Добавляем тестовую новость с изображением
                var unitsId = Guid.NewGuid();

                var units = new Units
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Description = "Testing",
                    Abbreviation = "kg",
                    Coefficient = 1.0m,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                };

                dbContext.Units.Add(units);
                dbContext.SaveChanges();
            });
        }
    }
}
