using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IntroductionToWebAPIs.BaseEntities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
