using System.ComponentModel.DataAnnotations;

namespace IntroductionToWebAPIs.DTO.LoginDTO
{
    public class LoginByPhoneRequest
    {
        [Required(ErrorMessage = "Номер телефона обязателен")]
        [Phone(ErrorMessage = "Некорректный формат номера")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; } = string.Empty;
    }
}
