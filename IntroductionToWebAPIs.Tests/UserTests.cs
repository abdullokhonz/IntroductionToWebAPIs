using IntroductionToWebAPIs.Entity.Users;
using IntroductionToWebAPIs.Enums.EnumUser;

namespace IntroductionToWebAPIs.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var login = "testuser";
            var email = "test@example.com";
            var password = "password123";

            // Act
            var user = new User(id, login, email, password);

            // Assert
            Assert.Equal(id, user.Id);
            Assert.Equal(login, user.Login);
            Assert.Equal(email, user.Email);
            Assert.False(user.IsConfirmed); // Проверяем значение по умолчанию
            Assert.Equal(UserType.Client, user.UserType); // Проверяем значение по умолчанию
            Assert.Equal(UserRole.Guest, user.Role); // Проверяем значение по умолчанию
            Assert.NotEqual(password, user.PasswordHash); // Проверяем, что пароль захэширован
            Assert.False(user.IsBlocked); // Проверяем значение по умолчанию
        }

        [Fact]
        public void HashPassword_ReturnsNonEmptyHashedPassword()
        {
            // Arrange
            var password = "password123";

            // Act
            var hashedPassword = User.HashPassword(password);

            // Assert
            Assert.False(string.IsNullOrEmpty(hashedPassword));
            Assert.NotEqual(password, hashedPassword); // Проверяем, что хэш отличается от исходного пароля
        }

        [Fact]
        public void VerifyPassword_ValidPassword_ReturnsTrue()
        {
            // Arrange
            var password = "password123";
            var hashedPassword = User.HashPassword(password);

            // Act
            var isValid = User.VerifyPassword(password, hashedPassword);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void VerifyPassword_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            var password = "password123";
            var wrongPassword = "wrongpassword";
            var hashedPassword = User.HashPassword(password);

            // Act
            var isValid = User.VerifyPassword(wrongPassword, hashedPassword);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void User_DefaultConstructor_SetsDefaultValues()
        {
            // Act
            var user = new User();

            // Assert
            Assert.NotEqual(Guid.Empty, user.Id); // Проверяем, что Id сгенерирован
            Assert.Equal(string.Empty, user.Login);
            Assert.Equal(string.Empty, user.Email);
            Assert.Equal(string.Empty, user.PasswordHash);
            Assert.Null(user.PhoneNumber);
            Assert.Equal(UserType.Client, user.UserType);
            Assert.Equal(UserRole.Guest, user.Role);
            Assert.Null(user.RefreshToken);
            Assert.False(user.IsConfirmed);
            Assert.False(user.IsPersonalDataAccepted);
            Assert.False(user.IsBlocked);
        }
    }
}
