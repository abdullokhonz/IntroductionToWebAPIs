using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : BaseController<Supplier>
    {
        public SupplierController(
            ILogger<BaseController<Supplier>> logger,
            IBaseService<Supplier> service) : base(logger, service)
        {
            
        }
    }
}
