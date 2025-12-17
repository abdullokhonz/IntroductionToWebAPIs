using System.Net;
using System.Net.Mail;

namespace IntroductionToWebAPIs.Services.Service
{
    public class EmailService
    {
        private readonly string? _fromEmail;
        private readonly string? _fromName;
        private readonly string? _password;
        private readonly string? _smtpHost;
        private readonly int _smtpPort;

        public EmailService(IConfiguration configuration)
        {
            _fromEmail = configuration["EmailSettings:FromEmail"];
            _fromName = configuration["EmailSettings:FromName"];
            _password = configuration["EmailSettings:EmailPassword"];
            _smtpHost = configuration["EmailSettings:SmtpHost"];
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]!);
        }

        // Синхронный метод для общей отправки email
        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                MailAddress from = new MailAddress(_fromEmail!, _fromName);
                MailAddress to = new MailAddress(toEmail);

                using (MailMessage msg = new MailMessage(from, to))
                {
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;

                    using (SmtpClient smtpClient = new SmtpClient(_smtpHost, _smtpPort))
                    {
                        smtpClient.Credentials = new NetworkCredential(_fromEmail, _password);
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(msg);
                    }
                }
            } 
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при отправке email: {ex.Message}");
            }
        }

        // Асинхронный метод для отправки кода подтверждения
        public async Task SendConfirmationEmailAsync(string email, string confirmationCode)
        {
            string subject = "Подтверждение регистрации";
            string body = $@"
                <h2>Подтверждение регистрации</h2>
                <p>Ваш код подтверждения: <strong>{confirmationCode}</strong></p>
                <p>Введите этот код для завершения регистрации.</p>";

            try
            {
                MailAddress from = new MailAddress(_fromEmail!, _fromName);
                MailAddress to = new MailAddress(email);

                using (MailMessage msg = new MailMessage(from, to))
                {
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;

                    using (SmtpClient smtpClient = new SmtpClient(_smtpHost, _smtpPort))
                    {
                        smtpClient.Credentials = new NetworkCredential(_fromEmail, _password);
                        smtpClient.EnableSsl = true;
                        await smtpClient.SendMailAsync(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при отправке email: {ex.Message}");
            }
        }
    }
}
