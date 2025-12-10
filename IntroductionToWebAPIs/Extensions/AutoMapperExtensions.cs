using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace IntroductionToWebAPIs.Extensions
{
    public static class AutoMapperExtensions
    {
        public static void ValidateAutoMapper(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            try
            {
                mapper.ConfigurationProvider.AssertConfigurationIsValid();
                Console.WriteLine("AutoMapper configuration is valid");
            }
            catch (AutoMapperConfigurationException ex)
            {
                Console.WriteLine("AutoMapper configuration error: " + ex.Message);
                throw;
            }
        }
    }
}
