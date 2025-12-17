using IntroductionToWebAPIs.Enums.EnumUser;

namespace IntroductionToWebAPIs.Contracts
{
    public class RequestCreateUser
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserType UserType { get; set; } = UserType.Client;
        public UserRole Role { get; set; } = UserRole.Guest;
    }
}
