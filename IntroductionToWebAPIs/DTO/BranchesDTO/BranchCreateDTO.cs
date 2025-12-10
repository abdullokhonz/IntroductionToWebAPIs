namespace IntroductionToWebAPIs.DTO.BranchesDTO
{
    public class BranchCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public Guid? ParentId { get; set; }
    }
}
