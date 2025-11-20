namespace IntroductionToWebAPIs.DTO
{
    public record PremiumCalculationResult(
        decimal BasePremium,
        decimal AgeFactor,
        decimal RegionFactor,
        decimal CarFactor,
        decimal ExperienceAndAccidentsFactor,
        decimal FinalPremium,
        string Explanation);
}
