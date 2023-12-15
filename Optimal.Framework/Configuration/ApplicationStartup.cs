using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimal.Framework.ConfigManager.DataConfig;
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
        }

    }
}
