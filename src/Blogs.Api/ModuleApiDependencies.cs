using Blogs.Core.Interfaces.Base;
using Blogs.DataAccess.Repositories.Base;

namespace Blogs.Api
{
    public static class ModuleApiDependencies
    {
        public static IServiceCollection AddApiDependencies(this IServiceCollection services)
        {

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
