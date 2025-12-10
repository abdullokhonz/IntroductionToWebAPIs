using IntroductionToWebAPIs.BaseEntities;
using System.Text.Json.Serialization;

namespace IntroductionToWebAPIs.Entity
{
    public class Branch : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public Guid? ParentId { get; set; }
        [JsonIgnore]
        public Branch? Parent { get; set; }
        public List<Branch> Children { get; set; } = new();
    }
}
