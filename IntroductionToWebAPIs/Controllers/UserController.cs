using Asp.Versioning;
using IntroductionToWebAPIs.Entity.Users;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class UserController : BaseController<User>
    {
        public UserController(
            ILogger<BaseController<User>> logger,
            IBaseService<User> service) : base(logger, service)
        {
            
        }
    }
}
