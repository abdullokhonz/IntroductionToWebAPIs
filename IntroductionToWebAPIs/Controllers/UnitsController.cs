using IntroductionToWebAPIs.Application.Queries.Units.GetAll;
using IntroductionToWebAPIs.Application.Queries.Units.GetById;
using IntroductionToWebAPIs.DTO.UnitsDTO;
using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Services.IService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    //[Authorize]
    public class UnitsController : BaseController<Units>
    {
        private readonly IMediator _mediator;

        public UnitsController(
            IMediator mediator,
            ILogger<BaseController<Units>> logger,
            IBaseService<Units> service) : base(logger, service)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllv2")]
        public async Task<ActionResult<IEnumerable<UnitsGetDTO>>> GetAllv2(CancellationToken ct = default) 
            => Ok(await _mediator.Send(new GetAllUnitsQuery(), ct));

        [HttpGet("GetByIdv2")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var dto = await _mediator.Send(new GetUnitsByIdQuery(id), ct);
            if (dto == null) return NotFound();
            return Ok(dto);
        }
    }
}
