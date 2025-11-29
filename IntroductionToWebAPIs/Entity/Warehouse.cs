using IntroductionToWebAPIs.BaseEntities;

namespace IntroductionToWebAPIs.Entity
{
    public class Warehouse : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
    }
}
