namespace IntroductionToWebAPIs.Services.IService
{
    public interface IBaseService<TEntity>
    {
        IQueryable<TEntity> GetAll();

        TEntity GetById(Guid id);

        string Create(TEntity item);

        string Update(Guid id, TEntity item);

        string Delete(Guid id);
    }
}
