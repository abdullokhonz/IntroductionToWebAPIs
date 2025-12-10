namespace IntroductionToWebAPIs.DTO.CategoriesDTO
{
    public class CategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
    }
}
