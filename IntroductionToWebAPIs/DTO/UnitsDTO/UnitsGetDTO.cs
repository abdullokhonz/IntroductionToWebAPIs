using IntroductionToWebAPIs.BaseEntities;

namespace IntroductionToWebAPIs.DTO.UnitsDTO
{
    public class UnitsGetDTO : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Abbreviation { get; set; } = null!;
        public decimal Coefficient { get; set; }
        public string? Description { get; set; }
    }
}
