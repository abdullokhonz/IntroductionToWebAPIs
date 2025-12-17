using IntroductionToWebAPIs.Auth;
using IntroductionToWebAPIs.Contracts;
using IntroductionToWebAPIs.Entity.Users;
using IntroductionToWebAPIs.Enums.EnumUser;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Services.IService;
using IntroductionToWebAPIs.Services.Service;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthByEmailController : ControllerBase
    {
        protected readonly AuthByEmailService _authService;
        private static Dictionary<string, string> _verificationCodes = new Dictionary<string, string>();
        private readonly EmailService _emailService;
        private readonly PostgreSQLDbContext _context;
        private readonly ISmsService _smsService;

        public AuthByEmailController(AuthByEmailService authService, EmailService emailService, PostgreSQLDbContext context, ISmsService smsService)
        {
            _authService = authService;
            _emailService = emailService;
            _context = context;
            _smsService = smsService;
        }


        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(
        Summary = "Аутентификация пользователя",
        Description = "Вход пользователя по email и паролю")]
        [SwaggerResponse(200, "Успешная аутентификация", typeof(TokenInfo))]
        [SwaggerResponse(400, "Неверные учетные данные")]

        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var tokenInfo = await _authService.LoginByEmail(request.Email, request.Password);
                return Ok(new
                {
                    accessToken = tokenInfo.AccessToken,
                    refreshToken = tokenInfo.RefreshToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, details = ex.StackTrace });
            }
        }

        [HttpPost("registerEmail")]
        public async Task<IActionResult> Register([FromBody] Contracts.RegisterRequest model)
        {
            try
            {
                // 1. Проверка согласия на обработку персональных данных
                if (!model.IsPersonalDataAccepted)
                {
                    return BadRequest("Необходимо согласие на обработку персональных данных.");
                }

                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest("Email и пароль обязательны.");
                }

                string code = CodeGeneratorService.GenerateVerificationCode();
                _verificationCodes[model.Email] = code;

                await _emailService.SendConfirmationEmailAsync(model.Email, code);

                // Сохраняем пользователя в базу
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Login = model.Login,
                    Email = model.Email,
                    Password = Entity.Users.User.HashPassword(model.Password),
                    IsConfirmed = false, // Новый пользователь не подтвержден
                    Role = UserRole.Guest, // роль по умолчанию
                    UserType = UserType.Client,
                    ConfirmationCode = code,
                    IsPersonalDataAccepted = model.IsPersonalDataAccepted
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // <-- Ошибка может быть здесь

                return Ok(new { message = "Код подтверждения отправлен на почту." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при регистрации: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        [HttpPost("verifyEmail")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyRequest model)
        {
            bool verified = await _authService.VerifyUser(model.Email, model.Code);

            if (!verified)
                return BadRequest("Неверный код.");

            // Получить пользователя после успешной верификации
            var user = await _authService.GetUserByEmail(model.Email);  // Метод для получения пользователя по email

            // Сгенерировать токены
            var tokens = await _authService.GeneratedJWT(user);

            // Возвратить токены
            return Ok(new { message = "Почта подтверждена!", tokens });
        }

        [HttpPost("sendSMS")]
        public async Task<IActionResult> SendSms([FromBody] SmsRequest request)
        {
            var result = await _smsService.SendSmsAsync(request.PhoneNumber, request.Message);
            return Ok(new { result });
        }
    }
}
