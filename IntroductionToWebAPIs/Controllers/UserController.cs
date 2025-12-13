using IntroductionToWebAPIs.Entity.Users;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController<User>
    {
        public UserController(
            ILogger<BaseController<User>> logger,
            IBaseService<User> service) : base(logger, service)
        {
            
        }
    }
}
