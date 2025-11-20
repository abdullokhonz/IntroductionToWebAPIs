using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
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

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _repository.GetAllAsync(ct);
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _repository.GetByIdAsync(id, ct);
        }

        public async Task<T> CreateAsync(T item, CancellationToken ct = default)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return await _repository.CreateAsync(item, ct);
        }

        public async Task<bool> UpdateAsync(Guid id, T item, CancellationToken ct = default)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            // Проверяем, существует ли запись
            var existing = await _repository.GetByIdAsync(id, ct);
            if (existing == null)
                return false;

            // Копируем данные из item в существующий объект (если нужно)
            // Здесь можно использовать AutoMapper или написать вручную
            // Например: CopyProperties(item, existing);

            return await _repository.UpdateAsync(item, ct);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await _repository.GetByIdAsync(id, ct);
            if (existing == null)
                return false;

            return await _repository.DeleteAsync(id, ct);
        }
    }
}
