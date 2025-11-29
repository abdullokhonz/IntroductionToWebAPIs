using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : BaseController<Warehouse>
    {
        public WarehouseController(
            ILogger<BaseController<Warehouse>> logger,
            IBaseService<Warehouse> service) : base(logger, service)
        {
        }
    }
}
