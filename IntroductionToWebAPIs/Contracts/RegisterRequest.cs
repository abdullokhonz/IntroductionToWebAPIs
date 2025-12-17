using System.ComponentModel.DataAnnotations;

namespace IntroductionToWebAPIs.Contracts
{
    public class RegisterRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(6, ErrorMessage = "Минимальная длина пароля — 6 символов")]
        public string Password { get; set; } = string.Empty;
        public bool IsPersonalDataAccepted { get; set; }
    }
}
