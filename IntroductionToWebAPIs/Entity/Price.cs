using IntroductionToWebAPIs.BaseEntities;
using System.Text.Json.Serialization;

namespace IntroductionToWebAPIs.Entity
{
    public class Price : BaseEntity
    {
        public decimal Value { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
