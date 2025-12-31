using IntroductionToWebAPIs.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IntroductionToWebAPIs.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly PostgreSQLDbContext _context;
        private readonly ILogger<DatabaseHealthCheck> _logger;

        public DatabaseHealthCheck(PostgreSQLDbContext context, ILogger<DatabaseHealthCheck> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                // Проверяем подключение к БД
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
                stopwatch.Stop();

                if (!canConnect)
                {
                    return HealthCheckResult.Unhealthy("Cannot connect to database");
                }

                // Дополнительная информация
                var data = new Dictionary<string, object>
        {
            { "connection_time_ms", stopwatch.ElapsedMilliseconds },
            { "database_name", _context.Database.GetDbConnection().Database }
        };

                _logger.LogInformation("Database health check passed in {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);

                return HealthCheckResult.Healthy("Database connection is healthy", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database health check failed");
                return HealthCheckResult.Unhealthy("Database connection failed", ex);
            }
        }
    }
}
