using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    public abstract class BaseController<TEntity> : ControllerBase
    {
        protected readonly IBaseService<TEntity> _service;
        protected readonly ILogger<BaseController<TEntity>> _logger;

        protected BaseController(ILogger<BaseController<TEntity>> logger, IBaseService<TEntity> service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("AllItems")]
        public async virtual Task<IEnumerable<TEntity>> Get(CancellationToken ct = default)
        {
            return await _service.GetAllAsync(ct);
        }

        [HttpGet("GetItemById")]
        public async virtual Task<TEntity> Get(Guid id, CancellationToken ct = default)
        {
            return await _service.GetByIdAsync(id, ct);
        }

        [HttpPost("CreateAsync")]
        public async virtual Task<TEntity> Post([FromBody] TEntity item, CancellationToken ct = default)
        {
            return await _service.CreateAsync(item, ct);
        }

        [HttpPut("UpdateAsync")]
        public async virtual Task<bool> Put([FromQuery] Guid id, [FromBody] TEntity item, CancellationToken ct = default)
        {
            return await _service.UpdateAsync(id, item, ct);
        }

        [HttpDelete("DeleteAsync")]
        // [Authorize(Roles = "admin")]
        public async virtual Task<bool> Delete([FromQuery] Guid id, CancellationToken ct = default)
        {
            return await _service.DeleteAsync(id, ct);
        }
    }
}
