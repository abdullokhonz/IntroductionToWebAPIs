using FluentValidation;
using IntroductionToWebAPIs.Auth;
using IntroductionToWebAPIs.Contracts;
using IntroductionToWebAPIs.DTO.LoginDTO;
using IntroductionToWebAPIs.DTO.UserDTO;
using IntroductionToWebAPIs.Entity.Users;
using IntroductionToWebAPIs.Enums.EnumUser;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Services.IService;
using IntroductionToWebAPIs.Services.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthByPhoneController : ControllerBase
    {
        protected readonly AuthByPhoneService _authByPhoneService;
        private static Dictionary<string, string> _verificationCodes = new Dictionary<string, string>();
        private readonly PostgreSQLDbContext _context;
        private readonly ISmsService _smsService;
        private readonly IValidator<RegisterRequest> _validator;

        public AuthByPhoneController(
            AuthByPhoneService authByPhoneService,
            PostgreSQLDbContext context,
            ISmsService smsService,
            IValidator<RegisterRequest> validator)
        {
            _authByPhoneService = authByPhoneService;
            _context = context;
            _smsService = smsService;
            _validator = validator;
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            try
            {
                TokenInfo token = await _authByPhoneService.RefreshToken(refreshToken);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("sendSMS")]
        public async Task<IActionResult> SendSms([FromBody] SmsRequest request)
        {
            var result = await _smsService.SendSmsAsync(request.PhoneNumber, request.Message);
            return Ok(new { result });
        }

        [HttpPost("registerPhone")]
        public async Task<IActionResult> RegisterByPhone([FromBody] RegisterByPhoneDTO model)
        {
            try
            {
                // 1. Проверка согласия
                if (!model.IsPersonalDataAccepted)
                {
                    return BadRequest("Необходимо согласие на обработку персональных данных.");
                }

                // 2. Проверка обязательных полей
                if (string.IsNullOrEmpty(model.PhoneNumber) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest("Номер телефона и пароль обязательны.");
                }

                // 3. Генерация и отправка кода
                string code = CodeGeneratorService.GenerateVerificationCode();
                _verificationCodes[model.PhoneNumber] = code;

                var smsResult = await _smsService.SendSmsAsync(model.PhoneNumber, $"Ваш код подтверждения: {code}");

                // 4. Сохранение пользователя
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Login = model.Login,
                    PhoneNumber = model.PhoneNumber,
                    Password = Entity.Users.User.HashPassword(model.Password),
                    IsConfirmed = false,
                    Role = UserRole.Guest,
                    UserType = UserType.Client,
                    ConfirmationCode = code,
                    IsPersonalDataAccepted = model.IsPersonalDataAccepted
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // 5. Ответ
                return Ok(new { message = "Код подтверждения отправлен по SMS." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при регистрации: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        [HttpPost("confirmPhone")]
        [SwaggerOperation(
            Summary = "Подтверждение кода",
            Description = "Подтверждение кода отправленного на телефон")]
        public async Task<IActionResult> ConfirmPhone([FromBody] VerifyRequestByPhone model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool verified = await _authByPhoneService.VerifyUserByPhone(model.PhoneNumber, model.Code);

            if (!verified)
                return BadRequest("Неверный код.");

            var user = await _authByPhoneService.GetUserByPhone(model.PhoneNumber);

            var tokens = await _authByPhoneService.GeneratedJWT(user);

            return Ok(new { message = "Телефон подтвержден!", tokens });
        }

        [HttpPost("loginByPhone")]
        [ProducesResponseType(typeof(TokenInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(
            Summary = "Аутентификация по номеру телефона",
            Description = "Вход пользователя по телефону и паролю")]
        [SwaggerResponse(200, "Успешная аутентификация", typeof(TokenInfo))]
        [SwaggerResponse(400, "Неверные учетные данные")]
        public async Task<IActionResult> LoginByPhone([FromBody] LoginByPhoneRequest request)
        {
            try
            {
                var tokenInfo = await _authByPhoneService.LoginByPhone(request.PhoneNumber, request.Password);
                return Ok(new
                {
                    accessToken = tokenInfo.AccessToken,
                    refreshToken = tokenInfo.RefreshToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
