using IntroductionToWebAPIs.DTO.PositionsDTO;
using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Responses;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace IntroductionToWebAPIs.Services.Service
{
    public class PositionService : IPositionService
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;
        IPostgreSQLRepository<Position> _repository;

        public PositionService(
            IPostgreSQLRepository<Position> repository,
            IConfiguration config,
            PostgreSQLDbContext context)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }



        public async Task<ServiceResponse<IEnumerable<Position>>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _repository.GetAllAsync(ct);

            if (result == null || !result.Any())
                return ServiceResponse<IEnumerable<Position>>.Fail("No items found");

            return ServiceResponse<IEnumerable<Position>>.Ok(result, "Items retrieved");
        }

        public async Task<ServiceResponse<Position?>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var existing = await _repository.GetByIdAsync(id, ct);

            if (existing == null)
                return ServiceResponse<Position?>.Fail("Item not found");

            return ServiceResponse<Position?>.Ok(existing, "Item retrieved");
        }

        public async Task<ServiceResponse<Position>> CreateAsync(Position item, CancellationToken ct = default)
        {
            if (item == null)
                return ServiceResponse<Position>.Fail("Item is null");

            var result = await _repository.CreateAsync(item, ct);

            if (result == null)
                return ServiceResponse<Position>.Fail("Failed to create item");

            return ServiceResponse<Position>.Ok(result, "Item created successfully");
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(Guid id, Position item, CancellationToken ct = default)
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
