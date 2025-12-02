using IntroductionToWebAPIs.Entity.Components;

namespace IntroductionToWebAPIs.DTO.CategoriesDTO
{
    public class CategoryTreeDto : ICategoryComponent
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CategoryTreeDto> Children { get; set; } = new();

        public void AddSub(ICategoryComponent category)
        {
            Children.Add((CategoryTreeDto)category);
        }

        public void RemoveSub(ICategoryComponent category)
        {
            Children.Remove((CategoryTreeDto)category);
        }

        public IEnumerable<ICategoryComponent> GetChildren() => Children;
    }
}
