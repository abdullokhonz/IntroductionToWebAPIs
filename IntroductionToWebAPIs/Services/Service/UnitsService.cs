using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToWebAPIs.Services.Service
{
    public class UnitsService : BaseService<Units>, IUnitsService
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;
        IPostgreSQLRepository<Units> _repository;

        public UnitsService(
            IPostgreSQLRepository<Units> repository,
            IConfiguration config,
            PostgreSQLDbContext context) : base(repository, config, context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Guid> AddAsync(Units entity, CancellationToken ct = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            // Защита от дубликатов по имени (по желанию)
            var exists = await _context.Units
                .AnyAsync(u => u.Name.ToLower() == entity.Name.Trim().ToLower(), ct);

            if (exists)
                throw new InvalidOperationException($"Единица измерения с именем '{entity.Name}' уже существует.");

            entity.CreatedAt = DateTime.UtcNow;

            await _context.Units.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);

            return entity.Id;
        }
    }
}
