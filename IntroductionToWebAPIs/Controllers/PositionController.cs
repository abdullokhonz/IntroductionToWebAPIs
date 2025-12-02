using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;
using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.DTO.CategoriesDTO;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionController : BaseController<Position>
    {
        private readonly IPositionService _positionService;

        public PositionController(
            ILogger<BaseController<Position>> logger,
            IBaseService<Position> service,
            IPositionService positionService) : base(logger, service)
        {
            _positionService = positionService;
        }

        [HttpGet("tree")]
        public async Task<IActionResult> GetTreeAsync()
        {
            var tree = await _positionService.GetPositionTreeAsync();
            return Ok(tree);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePositionAsync([FromBody] PositionCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _positionService.CreatePositionAsync(dto);
            return Ok(created);
        }
    }
}
