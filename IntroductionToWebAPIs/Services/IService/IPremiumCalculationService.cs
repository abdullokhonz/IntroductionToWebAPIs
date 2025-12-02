using IntroductionToWebAPIs.DTO;
using IntroductionToWebAPIs.Entity;

namespace IntroductionToWebAPIs.Services.IService
{
    public interface IPremiumCalculationService
    {
        Task<PremiumCalculationResult> CalculateAsync(Guid clientId, CancellationToken ct = default);

        Task<Client?> PremiumGetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
