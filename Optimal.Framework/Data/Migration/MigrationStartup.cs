using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimal.Framework.Infrastructure;

namespace Optimal.Framework.Data.Migration
{
    public class MigrationStartup : IOptimalStartup
    {
        public int Order => throw new NotImplementedException();

        public void Configure(IApplicationBuilder application)
        {
            using (var scope = application.ApplicationServices.CreateScope())
            {
                // var assembly = EngineContext.Current.Resolve<IEngine>().
                // var runner = scope.ServiceProvider.GetRequiredService<IMigrationManager>();
                // runner.ApplyUpMigrations(application.);
            }
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMigrationManager, MigrationManager>();
        }
    }
}