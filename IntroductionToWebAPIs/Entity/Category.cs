using IntroductionToWebAPIs.BaseEntities;
using System.Text.Json.Serialization;

namespace IntroductionToWebAPIs.Entity
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        [JsonIgnore]
        public Category? Parent { get; set; }
        public List<Category> Children { get; set; } = new();
    }
}
