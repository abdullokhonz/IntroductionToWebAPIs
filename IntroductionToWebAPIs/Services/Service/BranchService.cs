using IntroductionToWebAPIs.DTO.BranchesDTO;
using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToWebAPIs.Services.Service
{
    public class BranchService : BaseService<Branch>, IBranchService
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;
        IPostgreSQLRepository<Branch> _repository;

        public BranchService(
            IPostgreSQLRepository<Branch> repository,
            IConfiguration config,
            PostgreSQLDbContext context) : base(repository, config, context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<BranchTreeDTO>> GetBranchTreeAsync()
        {
            var all = await _context.Branches.ToListAsync();

            var lookup = all.ToDictionary(b => b.Id, b => new BranchTreeDTO
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Location = b.Location
            });

            List<BranchTreeDTO> roots = new();

            foreach (var b in all)
            {
                if (b.ParentId == null)
                {
                    roots.Add(lookup[b.Id]);
                }
                else
                {
                    if (lookup.TryGetValue(b.ParentId.Value, out var parent))
                    {
                        parent.AddSub(lookup[b.Id]);
                    }
                }
            }

            return roots;
        }

        public async Task<Branch> CreateBranchAsync(BranchCreateDTO dto)
        {
            var branch = new Branch
            {
                Name = dto.Name,
                Description = dto.Description,
                Location = dto.Location,
                ParentId = dto.ParentId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();

            return branch;
        }
    }
}
