using IntroductionToWebAPIs.BaseEntities;

namespace IntroductionToWebAPIs.Entity
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public uint Age { get; set; }
        public string? Bio { get; set; }
    }
}
