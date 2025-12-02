using IntroductionToWebAPIs.DTO.CategoriesDTO;
using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : BaseController<Category>
    {
        private readonly ICategoryService _categoriesService;

        public CategoryController(
            ILogger<BaseController<Category>> logger,
            IBaseService<Category> service,
            ICategoryService categoriesService) : base(logger, service)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet("tree")]
        public async Task<IActionResult> GetTree()
        {
            var tree = await _categoriesService.GetCategoryTreeAsync();
            return Ok(tree);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _categoriesService.CreateCategoryAsync(dto);
            return Ok(created);
        }
    }
}
