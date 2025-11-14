using IntroductionToWebAPIs.BaseEntities;

namespace IntroductionToWebAPIs.Entity
{
    public class Unit : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
