using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining(typeof(ServiceRegistration));
            //services.AddFluentValidationAutoValidation();
        }
    }
}
