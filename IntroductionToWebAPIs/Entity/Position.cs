using IntroductionToWebAPIs.BaseEntities;
using System.Text.Json.Serialization;

namespace IntroductionToWebAPIs.Entity
{
    public class Position : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? ParentId { get; set; }
        [JsonIgnore]
        public Position? Parent { get; set; }
        public List<Position> Children { get; set; } = new();
    }
}
