using IntroductionToWebAPIs.Entity;
using IntroductionToWebAPIs.Infrastructure;
using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Services.IService;

namespace IntroductionToWebAPIs.Services.Service
{
    public class SupplierService : ISupplierService
    {
        private readonly IConfiguration _config;
        private readonly PostgreSQLDbContext _context;
        IPostgreSQLRepository<Supplier> _repository;

        public SupplierService(
            IPostgreSQLRepository<Supplier> repository,
            IConfiguration config,
            PostgreSQLDbContext context)
        {
            _repository = repository;
            _config = config;
            _context = context;
        }



        public string Create(Supplier item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                return "The name cannot be empty";
            }
            else
            {
                _repository.Create(item);
                return $"Created new item with this ID: {item.Id}";
            }
        }

        public string Delete(Guid id)
        {
            var result = _repository.Delete(id);
            if (result)
                return "Item deleted";
            else
                return "Item not found";
        }

        public IQueryable<Supplier> GetAll()
        {
            return _repository.GetAll();
        }

        public Supplier GetById(Guid id)
        {
            return _repository.GetById(id); 
        }

        public string Update(Guid id, Supplier item)
        {
            var _item = _repository.GetById(id);

            if (_item is not null)
            {
                _item.Name = item.Name;



                var result = _repository.Update(_item);
                if (result)
                    return "Item updated";
            }

            return "Item not found";
        }
    }
}
