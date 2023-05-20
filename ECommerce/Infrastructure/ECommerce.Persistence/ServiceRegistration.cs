using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<ECommerceDbContext>(options =>
            {
                options.UseNpgsql(Configuration.ConnectionString);
            });
        }
    }
}
