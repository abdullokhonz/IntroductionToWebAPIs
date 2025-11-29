using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : BaseController<Category>
    {
        public CategoryController(
            ILogger<BaseController<Category>> logger,
            IBaseService<Category> service) : base(logger, service)
        {
        }
    }
}
