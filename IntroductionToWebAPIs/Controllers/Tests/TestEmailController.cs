using Asp.Versioning;
using IntroductionToWebAPIs.DTO.Tests;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace IntroductionToWebAPIs.Controllers
{
    [ApiController]
    [Route("api/test-email")]
    [ApiVersion("2.0")]
    public class TestEmailController : ControllerBase
    {
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendEmailRequest request)
        {
            // ⚠️ ТЕСТОВЫЕ ДАННЫЕ
            var fromEmail = "abdullokhon1206@mail.ru";
            var fromPassword = "rXtOFUYieQz9Rc8beiZD";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(request.Title, fromEmail)); // "My Test Project 🚀"
            message.To.Add(MailboxAddress.Parse(request.Email));
            message.Subject = request.Subject; // "Привет от моего проекта 👋"

            message.Body = new TextPart("plain")
            {
                Text = request.Message
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.mail.ru", 587, false);
            await client.AuthenticateAsync(fromEmail, fromPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return Ok("Письмо отправлено 😎");
        }
    }
}
