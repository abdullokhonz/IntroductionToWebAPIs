namespace IntroductionToWebAPIs.Contracts
{
    public class SmsRequest
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
