using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimal.Framework.Data.ConfigManager;
using Optimal.Framework.Data.DataProvider;
using Optimal.Framework.Infrastructure;

namespace Optimal.Framework.Data.Configuration
{
    public class DataConfigureServices : IOptimalStartup
    {
        public int Order => 2;

        public void Configure(IApplicationBuilder application)
        {
            throw new NotImplementedException();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAppDataProvider, BaseDataProvider>();
            DataSettingManager.LoadSettings(configuration);
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        }
    }
}
