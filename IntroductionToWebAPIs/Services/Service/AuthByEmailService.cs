using IntroductionToWebAPIs.Auth;
using IntroductionToWebAPIs.Entity.Users;
using IntroductionToWebAPIs.Exceptions;
using IntroductionToWebAPIs.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IntroductionToWebAPIs.Services.Service
{
    public class AuthByEmailService
    {
        private readonly PostgreSQLDbContext _context;
        private readonly EmailService _emailService;
        private readonly AuthOptions _authOptions;
        public AuthByEmailService(IOptions<AuthOptions> authOptions, PostgreSQLDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
            _authOptions = authOptions.Value;
        }

        public async Task<TokenInfo> Login(string username, string password)
        {
            // Найти пользователя по имени
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == username);

            if (user == null)
                throw new Exception("Пользователь не найден.");

            // Проверить пароль с помощью BCrypt.Verify
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new Exception("Неверный пароль.");

            // Если пароль корректен, вернуть токен (логика генерации токена остается)
            return await GeneratedJWT(user);
        }

        public async Task<TokenInfo> LoginByEmail(string email, string password)
        {
            // Find user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user == null)
                throw new UnauthorizedException("Invalid email or password.");

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new UnauthorizedException("Invalid email or password.");

            // Check if user is confirmed
            if (!user.IsConfirmed)
                throw new UnauthorizedException("Please verify your email before logging in.");

            // Check if user is blocked
            if (user.IsBlocked)
                throw new UnauthorizedException("Your account has been blocked. Please contact support.");

            var token = await GeneratedJWT(user);

            // Generate tokens
            return new TokenInfo
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken
            };
        }

        public async Task<TokenInfo> GeneratedJWT(User user)
        {
            if (user == null)
                throw new ArgumentException("Invalid username or password.");

            if (user.IsBlocked)
                throw new ArgumentException("User does not have access to login.");

            var claims = new List<Claim>
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login)
        };

            claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            var refreshToken = Guid.NewGuid().ToString();
            var userId = user.Id;

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Обновляем срок действия refresh-токена
            await _context.SaveChangesAsync();

            return new TokenInfo { AccessToken = accessToken, RefreshToken = refreshToken };
        }


        public async Task<bool> VerifyUser(string email, string code)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.ConfirmationCode != code)
                return false;

            user.IsConfirmed = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
