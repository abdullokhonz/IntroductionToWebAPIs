namespace IntroductionToWebAPIs.Repositories
{
    public interface IPostgreSQLRepository<T>
    {
        IQueryable<T> GetAll();
        T GetById(Guid id);
        bool Create(T item);
        bool Update(T item);
        bool Delete(Guid id);
    }
}
