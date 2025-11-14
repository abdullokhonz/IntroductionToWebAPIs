using IntroductionToWebAPIs.BaseEntities;
using IntroductionToWebAPIs.Infrastructure;

namespace IntroductionToWebAPIs.Repositories
{
    public class PostgreSQLRepository<T> : IPostgreSQLRepository<T> where T : BaseEntity
    {
        readonly PostgreSQLDbContext _context;
        public PostgreSQLRepository(PostgreSQLDbContext autoProductContext)
        {
            _context = autoProductContext;
        }
        public bool Create(T item)
        {
            try
            {
                Console.WriteLine($"Добавление в БД: {item}");
                _context.Add(item);
                var result = _context.SaveChanges();

                Console.WriteLine($"SaveChanges() выполнен, изменено {result} записей.");

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
                return false;
            }
        }


        public bool Delete(Guid id)
        {
            try
            {
                var item = _context.Set<T>().SingleOrDefault(w => w.Id == id);
                if (item is not null)
                {
                    _context.Remove(item);
                    var result = _context.SaveChanges();
                    return result > 0;
                }
            }
            catch
            { }

            return false;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T GetById(Guid id)
        {
            return _context.Set<T>().SingleOrDefault(w => w.Id == id)!;
        }

        public bool Update(T item)
        {
            try
            {
                _context.Update(item);
                var result = _context.SaveChanges();
                return result > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
