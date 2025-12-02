using IntroductionToWebAPIs.DTO;
using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Services.IService;

namespace IntroductionToWebAPIs.Services.Service
{
    public class PremiumCalculationService : IPremiumCalculationService
    {
        private const decimal BaseRate = 15000m; // базовая ставка ОСАГО-подобная
        private readonly IPostgreSQLRepository<Client> _clientRepository;

        public PremiumCalculationService(IPostgreSQLRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<PremiumCalculationResult> CalculateAsync(Guid clientId, CancellationToken ct = default)
        {
            var client = await PremiumGetByIdAsync(clientId);
            if (client == null)
                throw new KeyNotFoundException("Клиент не найден");

            // Имитируем тяжёлые расчёты — запускаем параллельно!
            var tasks = new[]
            {
                Task.Run(() => CalculateAgeRisk(client.Age), ct),
                Task.Run(() => CalculateRegionRisk(client.Region), ct),
                Task.Run(() => CalculateCarRisk(client.CarModel, client.CarPowerHp), ct),
                Task.Run(() => CalculateExperienceAndAccidentsRisk(client.DrivingExperienceYears, client.HasPreviousAccidents), ct)
            };

            var results = await Task.WhenAll(tasks);

            decimal ageFactor = results[0];
            decimal regionFactor = results[1];
            decimal carFactor = results[2];
            decimal expFactor = results[3];

            decimal finalPremium = BaseRate * ageFactor * regionFactor * carFactor * expFactor;

            var explanation =
                $"Базовая ставка: {BaseRate:N0} ₽\n" +
                $"× Возраст ({client.Age} лет): ×{ageFactor:F2}\n" +
                $"× Регион ({client.Region}): ×{regionFactor:F2}\n" +
                $"× Авто ({client.CarModel}, {client.CarPowerHp} л.с.): ×{carFactor:F2}\n" +
                $"× Опыт и аварии: ×{expFactor:F2}\n" +
                $"= ИТОГО: {finalPremium:N0} ₽";

            return new PremiumCalculationResult(
                BasePremium: BaseRate,
                AgeFactor: ageFactor,
                RegionFactor: regionFactor,
                CarFactor: carFactor,
                ExperienceAndAccidentsFactor: expFactor,
                FinalPremium: Math.Round(finalPremium),
                Explanation: explanation
            );
        }

        // Имитация тяжёлого CPU-расчёта (например, нейросеть, сложная формула, статистика)
        private decimal CalculateAgeRisk(int age)
        {
            Thread.Sleep(300); // имитируем нагрузку
            return age < 22 ? 2.5m : age > 65 ? 1.8m : 1.0m;
        }

        private decimal CalculateRegionRisk(string region)
        {
            Thread.Sleep(250);
            return region switch
            {
                "Moscow" or "SaintPetersburg" => 2.0m,
                "Siberia" or "FarEast" => 1.5m,
                _ => 1.1m
            };
        }

        private decimal CalculateCarRisk(string model, int powerHp)
        {
            Thread.Sleep(400);
            var baseFactor = powerHp switch
            {
                > 200 => 2.2m,
                > 150 => 1.7m,
                > 100 => 1.3m,
                _ => 1.0m
            };

            var modelFactor = model.Contains("BMW") || model.Contains("Mercedes") ? 1.4m : 1.0m;
            return baseFactor * modelFactor;
        }

        private decimal CalculateExperienceAndAccidentsRisk(int experienceYears, bool hasAccidents)
        {
            Thread.Sleep(200);
            var expFactor = experienceYears switch
            {
                < 3 => 2.0m,
                < 10 => 1.3m,
                _ => 0.9m
            };

            return hasAccidents ? expFactor * 1.8m : expFactor;
        }



        public async Task<Client?> PremiumGetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _clientRepository.GetByIdAsync(id, ct);
        }
    }
}
