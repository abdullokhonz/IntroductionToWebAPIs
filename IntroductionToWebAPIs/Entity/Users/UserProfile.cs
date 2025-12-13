using System.ComponentModel.DataAnnotations.Schema;

namespace IntroductionToWebAPIs.Entity.Users
{
    public class UserProfile
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid AddressId { get; set; }
        public string Address { get; set; }
        public string? PassportNumber { get; set; }
    }
}
