using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Services.IService;

namespace IntroductionToWebAPIs.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;
        IPostgreSQLRepository<User> _repository;

        public UserService(
            IPostgreSQLRepository<User> repository,
            IConfiguration config,
            PostgreSQLDbContext context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }



        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default)
        {
            return await _repository.GetAllAsync(ct);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _repository.GetByIdAsync(id, ct);
        }

        public async Task<User> CreateAsync(User item, CancellationToken ct = default)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var result = await _repository.CreateAsync(item, ct);

            return result;
        }

        public async Task<bool> UpdateAsync(Guid id, User item, CancellationToken ct = default)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var existing = await _repository.GetByIdAsync(id, ct);
            if (existing == null)
                return false;

            var result = await _repository.UpdateAsync(item, ct);

            return result;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await _repository.GetByIdAsync(id, ct);
            if (existing == null)
                return false;

            var result = await _repository.DeleteAsync(existing.Id, ct);

            return result;
        }
    }
}
