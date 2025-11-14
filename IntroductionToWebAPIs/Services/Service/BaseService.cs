using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Services.IService;

namespace IntroductionToWebAPIs.Services.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;

        IPostgreSQLRepository<T> _repository;

        public BaseService(IPostgreSQLRepository<T> repository, IConfiguration config, PostgreSQLDbContext context)
        {
            _repository = repository;
            _config = config;
            _context = context;
        }



        public string Create(T item)
        {
            var nameProperty = typeof(T).GetProperty("Name");
            if (nameProperty != null)
            {
                var nameValue = nameProperty.GetValue(item) as string;
                if (string.IsNullOrEmpty(nameValue))
                {
                    return "The name cannot be empty";
                }
            }

            _repository.Create(item);
            return $"Created new item with this ID: {item}";
        }

        public string Delete(Guid id)
        {
            var result = _repository.Delete(id);
            if (result)
                return "Item deleted";
            else
                return "Item not found";
        }

        public IQueryable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        public string Update(Guid id, T item)
        {
            var _item = _repository.GetById(id);
            if (_item is not null)
            {
                _item = item;



                var result = _repository.Update(_item);
                if (result)
                    return "Item updated";
            }

            return "Item updated";
        }
    }
}
