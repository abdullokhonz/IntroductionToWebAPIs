using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseController<Product>
    {
        public ProductController(
            ILogger<BaseController<Product>> logger,
            IBaseService<Product> service) : base(logger, service)
        {
        }
    }
}
