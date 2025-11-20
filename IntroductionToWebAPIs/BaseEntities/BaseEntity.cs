using System.ComponentModel.DataAnnotations;

namespace IntroductionToWebAPIs.BaseEntities
{
    public class BaseEntity
    {
        [Key]
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
