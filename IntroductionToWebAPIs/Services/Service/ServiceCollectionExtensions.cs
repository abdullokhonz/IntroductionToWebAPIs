using IntroductionToWebAPIs.Repositories;
using IntroductionToWebAPIs.Services.IService;

namespace IntroductionToWebAPIs.Services.Service
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMyServices(this IServiceCollection service)
        {
            service.AddScoped<IUnitsService, UnitsService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<ISupplierService, SupplierService>();
            service.AddScoped<IPremiumCalculationService, PremiumCalculationService>();
            service.AddScoped<ICategoryService, CategoryService>();
            service.AddScoped<IWarehouseService, WarehouseService>();
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<IPriceService, PriceService>();
            service.AddScoped<IPositionService, PositionService>();
            service.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            service.AddScoped(typeof(IPostgreSQLRepository<>), typeof(PostgreSQLRepository<>));

            service.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            service.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
