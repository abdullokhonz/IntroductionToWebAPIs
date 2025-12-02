namespace IntroductionToWebAPIs.DTO.CategoriesDTO
{
    public class PositionCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
    }
}
