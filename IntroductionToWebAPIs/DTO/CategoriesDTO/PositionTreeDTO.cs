using IntroductionToWebAPIs.Entity.Components;

namespace IntroductionToWebAPIs.DTO.CategoriesDTO
{
    public class PositionTreeDTO : IPositionComponent
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<PositionTreeDTO> Children { get; set; } = new();

        public void AddSub(IPositionComponent position)
        {
            Children.Add((PositionTreeDTO)position);
        }

        public void RemoveSub(IPositionComponent position)
        {
            Children.Remove((PositionTreeDTO)position);
        }

        public IEnumerable<IPositionComponent> GetChildren() => Children;
    }
}
    