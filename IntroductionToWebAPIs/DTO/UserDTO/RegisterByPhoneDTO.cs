using System.ComponentModel.DataAnnotations;

namespace IntroductionToWebAPIs.DTO.UserDTO
{
    public class RegisterByPhoneDTO
    {
        /// <summary>
        /// Номер телефона (в международном формате, например, +992900001111)
        /// </summary>
        [Required(ErrorMessage = "Номер телефона обязателен")]
        [Phone(ErrorMessage = "Некорректный формат номера телефона")]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Пароль для входа
        /// </summary>
        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(6, ErrorMessage = "Минимальная длина пароля — 6 символов")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Подтверждение пароля
        /// </summary>
        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение не совпадают")]
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// Имя пользователя (необязательно)
        /// </summary>
        public string? Login { get; set; }

        /// <summary>
        /// Согласие на обработку персональных данных
        /// </summary>
        [Required(ErrorMessage = "Необходимо согласиться на обработку персональных данных")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Необходимо согласие на обработку персональных данных")]
        public bool IsPersonalDataAccepted { get; set; }
    }
}
