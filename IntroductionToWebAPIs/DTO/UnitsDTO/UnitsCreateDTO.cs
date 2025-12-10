namespace IntroductionToWebAPIs.DTO.UnitsDTO
{
    public class UnitsCreateDTO
    {
        public string Name { get; set; } = null!;
        public string Abbreviation { get; set; } = null!;
        public decimal Coefficient { get; set; } = 1;
        public string? Description { get; set; }
    }
}
