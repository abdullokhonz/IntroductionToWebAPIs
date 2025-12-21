using IntroductionToWebAPIs.Auth;
using IntroductionToWebAPIs.Entity.Users;
using IntroductionToWebAPIs.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IntroductionToWebAPIs.Services.Service
{
    public class AuthByPhoneService
    {
        private readonly PostgreSQLDbContext _context;

        public AuthByPhoneService(PostgreSQLDbContext context)
        {
            _context = context;

        }

        public async Task<TokenInfo> RefreshToken(string refreshToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new ArgumentException("Invalid or expired refresh token.");

            return await GeneratedJWT(user);
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

        public async Task<User?> GetUserByPhone(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<TokenInfo> LoginByPhone(string phoneNumber, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user == null)
                throw new Exception("Пользователь с таким номером не найден");

            if (!user.IsConfirmed)
                throw new Exception("Номер телефона не подтвержден");

            if (!User.VerifyPassword(password, user.Password))
                throw new Exception("Неверный пароль");

            return await GeneratedJWT(user);
        }

        public async Task<bool> VerifyUserByPhone(string phoneNumber, string code)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

            if (user == null || user.ConfirmationCode != code)
                return false;

            user.IsConfirmed = true;
            user.ConfirmationCode = string.Empty;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
