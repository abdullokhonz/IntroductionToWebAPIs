using System.ComponentModel.DataAnnotations;

namespace IntroductionToWebAPIs.Contracts
{
    public class VerifyRequestByPhone
    {
        [Required(ErrorMessage = "Номер телефона обязателен")]
        [Phone(ErrorMessage = "Некорректный формат номера телефона")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Код подтверждения обязателен")]
        public string Code { get; set; } = string.Empty;
    }
}
