using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Responses;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToWebAPIs.Services.Service
{
    public class UnitsService : IUnitsService
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;
        IPostgreSQLRepository<Units> _repository;

        public UnitsService(
            IPostgreSQLRepository<Units> repository,
            IConfiguration config,
            PostgreSQLDbContext context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }



        public async Task<ServiceResponse<IEnumerable<Units>>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _repository.GetAllAsync(ct);

            if (result == null || !result.Any())
                return ServiceResponse<IEnumerable<Units>>.Fail("No items found");

            return ServiceResponse<IEnumerable<Units>>.Ok(result, "Items retrieved");
        }

        public async Task<ServiceResponse<Units?>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await _repository.GetByIdAsync(id, ct);

            if (existing == null)
                return ServiceResponse<Units?>.Fail("Item not found");

            return ServiceResponse<Units?>.Ok(existing, "Item retrieved");
        }

        public async Task<ServiceResponse<Units>> CreateAsync(Units item, CancellationToken ct = default)
        {
            if (item == null)
                return ServiceResponse<Units>.Fail("Item is null");

            var result = await _repository.CreateAsync(item, ct);

            if (result == null)
                return ServiceResponse<Units>.Fail("Failed to create item");

            return ServiceResponse<Units>.Ok(result, "Item created successfully");
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(Guid id, Units item, CancellationToken ct = default)
        {
            if (item == null)
                return ServiceResponse<bool>.Fail("Item is null");

            var existing = await _repository.GetByIdAsync(id, ct);

            if (existing == null)
                return ServiceResponse<bool>.Fail("Item not found");

            var result = await _repository.UpdateAsync(item, ct);

            if (!result)
                return ServiceResponse<bool>.Fail("Failed to update item");

            return ServiceResponse<bool>.Ok(true, "Item updated successfully");
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await _repository.GetByIdAsync(id, ct);

            if (existing == null)
                return ServiceResponse<bool>.Fail("Item not found");

            var result = await _repository.DeleteAsync(id, ct);

            if (!result)
                return ServiceResponse<bool>.Fail("Failed to delete item");

            return ServiceResponse<bool>.Ok(true, "Item deleted successfully");
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
