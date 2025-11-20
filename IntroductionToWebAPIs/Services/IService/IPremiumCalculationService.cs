using IntroductionToWebAPIs.DTO;

namespace IntroductionToWebAPIs.Services.IService
{
    public interface IPremiumCalculationService
    {
        Task<PremiumCalculationResult> CalculateAsync(Guid clientId, CancellationToken ct = default);
    }
}
