using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : BaseController<Branch>
    {
        private readonly IBranchService _branchService;

        public BranchController(
            ILogger<BaseController<Branch>> logger,
            IBaseService<Branch> service,
            IBranchService branchService) : base(logger, service)
        {
            _branchService = branchService ?? throw new ArgumentNullException(nameof(branchService));
        }

        [HttpGet("tree")]
        public async Task<IActionResult> GetBranchTreeAsync(CancellationToken ct = default)
        {
            var result = await _branchService.GetBranchTreeAsync();
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBranchAsync([FromBody] DTO.BranchesDTO.BranchCreateDTO dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _branchService.CreateBranchAsync(dto);
            return Ok(created);
        }
    }
}
