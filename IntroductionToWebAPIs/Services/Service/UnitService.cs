using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Responses;
using IntroductionToWebAPIs.Services.IService;

namespace IntroductionToWebAPIs.Services.Service
{
    public class UnitService : IUnitService
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;
        IPostgreSQLRepository<Unit> _repository;

        public UnitService(
            IPostgreSQLRepository<Unit> repository,
            IConfiguration config,
            PostgreSQLDbContext context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }



        public async Task<ServiceResponse<IEnumerable<Unit>>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _repository.GetAllAsync(ct);

            return ServiceResponse<IEnumerable<Unit>>.Ok(result, "Items retrieved");
        }

        public async Task<ServiceResponse<Unit?>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await _repository.GetByIdAsync(id, ct);

            if (existing == null)
                return ServiceResponse<Unit?>.Fail("Item not found");

            return ServiceResponse<Unit?>.Ok(existing, "Item retrieved");
        }

        public async Task<ServiceResponse<Unit>> CreateAsync(Unit item, CancellationToken ct = default)
        {
            if (item == null)
                return ServiceResponse<Unit>.Fail("Item is null");

            var result = await _repository.CreateAsync(item, ct);

            if (result == null)
                return ServiceResponse<Unit>.Fail("Failed to create item");

            return ServiceResponse<Unit>.Ok(result, "Item created successfully");
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(Guid id, Unit item, CancellationToken ct = default)
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
    }
}
