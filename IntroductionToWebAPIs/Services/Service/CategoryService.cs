using IntroductionToWebAPIs.DTO.CategoriesDTO;
using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToWebAPIs.Services.Service
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;
        IPostgreSQLRepository<Category> _repository;

        public CategoryService(
            IPostgreSQLRepository<Category> repository,
            IConfiguration config,
            PostgreSQLDbContext context) : base(repository, config, context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
    }
}
