using IntroductionToWebAPIs.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace IntroductionToWebAPIs.Entity
{
    public class Client : BaseEntity
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public int DrivingExperienceYears { get; set; }

        [MaxLength(100)]
        public string Region { get; set; } = string.Empty;

        [MaxLength(100)]
        public string CarModel { get; set; } = string.Empty;

        public int CarPowerHp { get; set; }

        public bool HasPreviousAccidents { get; set; }

        public DateTime LicenseIssuedDate { get; set; } = DateTime.UtcNow;
    }
}
