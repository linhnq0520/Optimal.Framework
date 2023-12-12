using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimal.Framework.Data;

namespace Optimal.Framework.Configuration
{
    public static class ApplicationStartup
    {
        public static IServiceCollection ApplicationConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
            });
            return services;
        }
    }
}
