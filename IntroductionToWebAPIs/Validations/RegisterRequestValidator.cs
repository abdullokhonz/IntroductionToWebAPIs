using FluentValidation;
using IntroductionToWebAPIs.Infrastructure;
using Microsoft.EntityFrameworkCore;
using IntroductionToWebAPIs.Contracts;

namespace IntroductionToWebAPIs.Validations
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        private readonly PostgreSQLDbContext _context;

        public RegisterRequestValidator(PostgreSQLDbContext context)
        {
            _context = context;

            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Логин обязателен")
                .Length(3, 50).WithMessage("Логин должен содержать от 3 до 50 символов")
                .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Логин может содержать только буквы, цифры и подчеркивание")
                .MustAsync(BeUniqueLogin).WithMessage("Этот логин уже используется");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Неверный формат email")
                .MaximumLength(100).WithMessage("Email не может быть длиннее 100 символов")
                .MustAsync(BeUniqueEmail).WithMessage("Этот email уже зарегистрирован");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(7).WithMessage("Минимальная длина пароля — 7 символов")
                .MaximumLength(100).WithMessage("Максимальная длина пароля — 100 символов")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
                .WithMessage("Пароль должен содержать как минимум одну строчную букву, одну заглавную букву и одну цифру");

            RuleFor(x => x.IsPersonalDataAccepted)
                .Equal(true).WithMessage("Необходимо согласие на обработку персональных данных");
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken token)
        {
            if (string.IsNullOrEmpty(email))
                return true; // Пустой email будет обработан другим правилом

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), token);

            return existingUser == null;
        }

        private async Task<bool> BeUniqueLogin(string login, CancellationToken token)
        {
            if (string.IsNullOrEmpty(login))
                return true; // Пустой логин будет обработан другим правилом

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Login.ToLower() == login.ToLower(), token);

            return existingUser == null;
        }
    }
}
