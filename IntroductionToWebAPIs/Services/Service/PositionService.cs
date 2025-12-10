using IntroductionToWebAPIs.DTO.PositionsDTO;
using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToWebAPIs.Services.Service
{
    public class PositionService : BaseService<Position>, IPositionService
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;
        IPostgreSQLRepository<Position> _repository;

        public PositionService(
            IPostgreSQLRepository<Position> repository,
            IConfiguration config,
            PostgreSQLDbContext context) : base(repository, config, context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<PositionTreeDTO>> GetPositionTreeAsync()
        {
            var all = await _context.Positions.ToListAsync();

            var lookup = all.ToDictionary(p => p.Id, p => new PositionTreeDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            });

            List<PositionTreeDTO> roots = new();

            foreach (var p in all)
            {
                if (p.ParentId == null)
                {
                    roots.Add(lookup[p.Id]);
                }
                else
                {
                    if (lookup.TryGetValue(p.ParentId.Value, out var parent))
                    {
                        parent.AddSub(lookup[p.Id]);
                    }
                }
            }

            return roots;
        }

        public async Task<Position> CreatePositionAsync(PositionCreateDTO dto)
        {
            var positon = new Position
            {
                Name = dto.Name,
                Description = dto.Description,
                ParentId = dto.ParentId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Positions.Add(positon);
            await _context.SaveChangesAsync();

            return positon;
        }
    }
}
