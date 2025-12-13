namespace IntroductionToWebAPIs.Services.Service
{
    public class CodeGeneratorService
    {
        public static string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(10000, 99999).ToString();
        }
    }
}
