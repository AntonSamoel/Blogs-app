using Blogs.Services.Implementations;
using Blogs.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Blogs.Services
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            return services;
        }
    }
}
