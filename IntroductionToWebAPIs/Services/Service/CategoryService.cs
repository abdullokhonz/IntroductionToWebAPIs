using IntroductionToWebAPIs.DTO.CategoriesDTO;
using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Responses;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToWebAPIs.Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;
        IPostgreSQLRepository<Category> _repository;

        public CategoryService(
            IPostgreSQLRepository<Category> repository,
            IConfiguration config,
            PostgreSQLDbContext context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }



        public async Task<ServiceResponse<IEnumerable<Category>>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _repository.GetAllAsync(ct);

            return ServiceResponse<IEnumerable<Category>>.Ok(result, "Items retrieved");
        }

        public async Task<ServiceResponse<Category?>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await _repository.GetByIdAsync(id, ct);

            if (existing == null)
                return ServiceResponse<Category?>.Fail("Item not found");

            return ServiceResponse<Category?>.Ok(existing, "Item retrieved");
        }

        public async Task<ServiceResponse<Category>> CreateAsync(Category item, CancellationToken ct = default)
        {
            if (item == null)
                return ServiceResponse<Category>.Fail("Item is null");

            var result = await _repository.CreateAsync(item, ct);

            if (result == null)
                return ServiceResponse<Category>.Fail("Failed to create item");

            return ServiceResponse<Category>.Ok(result, "Item created successfully");
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(Guid id, Category item, CancellationToken ct = default)
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



        public async Task<IEnumerable<CategoryTreeDto>> GetCategoryTreeAsync()
        {
            var all = await _context.Categories.ToListAsync();

            // Создаём словарь
            var lookup = all.ToDictionary(c => c.Id, c => new CategoryTreeDto
            {
                Id = c.Id,
                Name = c.Name
            });

            List<CategoryTreeDto> roots = new();

            foreach (var c in all)
            {
                if (c.ParentId == null)
                {
                    // корневая категория
                    roots.Add(lookup[c.Id]);
                }
                else
                {
                    if (lookup.TryGetValue(c.ParentId.Value, out var parent))
                    {
                        parent.AddSub(lookup[c.Id]);
                    }
                }
            }

            return roots;
        }

        public async Task<Category> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                ParentId = dto.ParentId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }



        public async Task<Category?> PremiumGetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _repository.GetByIdAsync(id, ct);
        }
    }
}
