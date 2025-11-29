using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceController : BaseController<Price>
    {
        public PriceController(
            ILogger<BaseController<Price>> logger,
            IBaseService<Price> service) : base(logger, service)
        {
        }
    }
}
