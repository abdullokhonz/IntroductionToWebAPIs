using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Responses;
using IntroductionToWebAPIs.Services.IService;

namespace IntroductionToWebAPIs.Services.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;

        IPostgreSQLRepository<T> _repository;

        public BaseService(
            IPostgreSQLRepository<T> repository,
            IConfiguration config,
            PostgreSQLDbContext context
            )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ServiceResponse<IEnumerable<T>>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _repository.GetAllAsync(ct);

            if (result == null || !result.Any())
                return ServiceResponse<IEnumerable<T>>.Fail("No items found");

            return ServiceResponse<IEnumerable<T>>.Ok(result, "Items retrieved");
        }

        public async Task<ServiceResponse<T?>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await _repository.GetByIdAsync(id, ct);

            if (existing == null)
                return ServiceResponse<T?>.Fail("Item not found");

            return ServiceResponse<T?>.Ok(existing, "Item retrieved");
        }

        public async Task<ServiceResponse<T>> CreateAsync(T item, CancellationToken ct = default)
        {
            if (item == null)
                return ServiceResponse<T>.Fail("Item is null");

            var result = await _repository.CreateAsync(item, ct);

            if (result == null)
                return ServiceResponse<T>.Fail("Failed to create item");

            return ServiceResponse<T>.Ok(result, "Item created successfully");
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(Guid id, T item, CancellationToken ct = default)
        {
            if (item == null)
                return ServiceResponse<bool>.Fail("Item is null");

            // Проверяем, существует ли запись
            var existing = await _repository.GetByIdAsync(id, ct);

            if (existing == null)
                return ServiceResponse<bool>.Fail("Item not found");

            var result = await _repository.UpdateAsync(item, ct);

            if (!result)
                return ServiceResponse<bool>.Fail("Failed to update item");

            // Копируем данные из item в существующий объект (если нужно)
            // Здесь можно использовать AutoMapper или написать вручную
            // Например: CopyProperties(item, existing);

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
