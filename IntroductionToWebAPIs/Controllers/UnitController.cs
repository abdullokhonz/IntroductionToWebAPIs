using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[AllowAnonymous]
    //[Authorize]
    public class UnitController : BaseController<Unit>
    {
        public UnitController(
            ILogger<BaseController<Unit>> logger,
            IBaseService<Unit> service) : base(logger, service)
        {
        }
    }

}
