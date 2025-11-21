using IntroductionToWebAPIs.Responses;

namespace IntroductionToWebAPIs.Services.IService
{
    public interface IBaseService<TEntity>
    {
        Task<ServiceResponse<IEnumerable<TEntity>>> GetAllAsync(CancellationToken ct = default);

        Task<ServiceResponse<TEntity?>> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<ServiceResponse<TEntity>> CreateAsync(TEntity item, CancellationToken ct = default);

        Task<ServiceResponse<bool>> UpdateAsync(Guid id, TEntity item, CancellationToken ct = default);

        Task<ServiceResponse<bool>> DeleteAsync(Guid id, CancellationToken ct = default);



        Task<TEntity?> PremiumGetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
 