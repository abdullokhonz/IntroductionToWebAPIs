using IntroductionToWebAPIs.Entity;

namespace IntroductionToWebAPIs.Services.IService
{
    public interface IUnitsService : IBaseService<Units>
    {
        Task<Guid> AddAsync(Units entity, CancellationToken ct = default);

    }
}
