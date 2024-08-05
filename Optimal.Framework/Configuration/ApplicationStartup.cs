using Microsoft.Extensions.DependencyInjection;
using Optimal.Framework.Data.ConfigManager;
using Optimal.Framework.Data;
using Optimal.Framework.Data.DataProvider;
using Optimal.Framework.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace Optimal.Framework.Configuration
{
    public static class ApplicationStartup
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            Singleton<ITypeFinder>.Instance = new TypeFinder();
            services.AddSingleton(Singleton<ITypeFinder>.Instance);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAppDataProvider, BaseDataProvider>();
            DataSettingManager.LoadSettings(builder.Configuration);
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            //services.AddScoped<IJwtTokenService, JwtTokenService>();
            IEngine engine = EngineContext.Create();
            engine.ConfigureServices(services, builder.Configuration);
        }

    }
}
