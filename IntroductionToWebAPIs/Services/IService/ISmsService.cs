namespace IntroductionToWebAPIs.Services.IService
{
    public interface ISmsService
    {
        Task<string> SendSmsAsync(string phoneNumber, string message);
    }
}
