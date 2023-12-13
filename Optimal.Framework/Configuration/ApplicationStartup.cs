using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimal.Framework.Data;
using Optimal.Framework.Infrastructure;

namespace Optimal.Framework.Configuration
{
    public static class ApplicationStartup
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            Singleton<ITypeFinder>.Instance = new TypeFinder();
            services.AddSingleton(Singleton<ITypeFinder>.Instance);
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
            });
        }

    }
}
