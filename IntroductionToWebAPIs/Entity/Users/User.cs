using IntroductionToWebAPIs.BaseEntities;
using IntroductionToWebAPIs.Enums.EnumUser;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IntroductionToWebAPIs.Entity.Users
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Имя пользователя обязательно для заполнения")]
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = null;
        public string Password { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public UserType UserType { get; set; } = UserType.Client; // Default to Client type
        public UserRole Role { get; set; } = UserRole.Guest;
        public string? RefreshToken { get; set; }
        [JsonIgnore]
        public DateTime RefreshTokenExpiryTime { get; set; }

        // Добавляем поле для хранения кода подтверждения
        public string ConfirmationCode { get; set; }
        // Флаг подтверждения пользователя (false по умолчанию)
        public bool IsConfirmed { get; set; } = false;
        public bool IsPersonalDataAccepted { get; set; }

        public bool IsBlocked { get; set; }

        public UserProfile Profile { get; set; }


        public User() { }

        public User(Guid id, string login, string email, string password)
        {
            Id = id;
            Login = login;
            Email = email;
            Password = password;
            Password = HashPassword(password); // Хэшируем пароль перед сохранением
            IsConfirmed = false; // По умолчанию новый пользователь не подтвержден
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Метод проверки пароля
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
