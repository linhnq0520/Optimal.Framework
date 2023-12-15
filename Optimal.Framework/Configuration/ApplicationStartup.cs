using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimal.Framework.Data.ConfigManager;
using Optimal.Framework.Data;
using Optimal.Framework.Data.DataProvider;
using Optimal.Framework.Infrastructure;

namespace Optimal.Framework.Configuration
{
    public static class ApplicationStartup
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            Singleton<ITypeFinder>.Instance = new TypeFinder();
            services.AddSingleton(Singleton<ITypeFinder>.Instance);
            services.AddSingleton<IAppDataProvider, BaseDataProvider>();
            DataSettingManager.LoadSettings(configuration);
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        }

    }
}
