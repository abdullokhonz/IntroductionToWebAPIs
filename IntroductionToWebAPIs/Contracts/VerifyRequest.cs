namespace IntroductionToWebAPIs.Contracts
{
    public class VerifyRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
