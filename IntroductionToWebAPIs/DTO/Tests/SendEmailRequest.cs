namespace IntroductionToWebAPIs.DTO.Tests
{
    public class SendEmailRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
    }
}
