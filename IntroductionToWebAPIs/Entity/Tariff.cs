using IntroductionToWebAPIs.BaseEntities;

namespace IntroductionToWebAPIs.Entity
{
    public class Tariff : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
