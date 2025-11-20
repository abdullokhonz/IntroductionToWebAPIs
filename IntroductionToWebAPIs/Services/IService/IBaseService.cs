namespace IntroductionToWebAPIs.Services.IService
{
    public interface IBaseService<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);

        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<TEntity> CreateAsync(TEntity item, CancellationToken ct = default);

        Task<bool> UpdateAsync(Guid id, TEntity item, CancellationToken ct = default);

        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
 