using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Responses;
using IntroductionToWebAPIs.Services.IService;

namespace IntroductionToWebAPIs.Services.Service
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;
        IPostgreSQLRepository<Warehouse> _repository;

        public WarehouseService(
            IPostgreSQLRepository<Warehouse> repository,
            IConfiguration config,
            PostgreSQLDbContext context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }



        public async Task<ServiceResponse<IEnumerable<Warehouse>>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _repository.GetAllAsync(ct);

            return ServiceResponse<IEnumerable<Warehouse>>.Ok(result, "Items retrieved");
        }

        public async Task<ServiceResponse<Warehouse?>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await _repository.GetByIdAsync(id, ct);

            if (existing == null)
                return ServiceResponse<Warehouse?>.Fail("Item not found");

            return ServiceResponse<Warehouse?>.Ok(existing, "Item retrieved");
        }

        public async Task<ServiceResponse<Warehouse>> CreateAsync(Warehouse item, CancellationToken ct = default)
        {
            if (item == null)
                return ServiceResponse<Warehouse>.Fail("Item is null");

            var result = await _repository.CreateAsync(item, ct);

            if (result == null)
                return ServiceResponse<Warehouse>.Fail("Failed to create item");

            return ServiceResponse<Warehouse>.Ok(result, "Item created successfully");
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(Guid id, Warehouse item, CancellationToken ct = default)
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
