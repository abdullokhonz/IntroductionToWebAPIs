using IntroductionToWebAPIs.DTO.CategoriesDTO;
using IntroductionToWebAPIs.Entity;

namespace IntroductionToWebAPIs.Services.IService
{
    public interface ICategoryService : IBaseService<Category>
    {
        Task<IEnumerable<CategoryTreeDto>> GetCategoryTreeAsync();

        Task<Category> CreateCategoryAsync(CategoryCreateDto dto);
    }
}
