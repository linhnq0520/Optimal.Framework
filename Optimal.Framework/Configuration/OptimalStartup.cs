using Microsoft.Extensions.DependencyInjection;
using Optimal.Framework.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Optimal.Framework.Configuration
{
    public static class OptimalStartup
    {
        public static void ConfigureApplicationServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddHttpContextAccessor();
            TypeFinder typeFinder = (TypeFinder)(Singleton<ITypeFinder>.Instance = new TypeFinder());
            services.AddSingleton((ITypeFinder) typeFinder);
            List<IConfig> list = (from configType in typeFinder.FindClassesOfType<IConfig>()
                                  select (IConfig)Activator.CreateInstance(configType)).ToList();
            AppSettings implementationInstance = null;
            foreach (IConfig item in list)
            {
                builder.Configuration.GetSection(item.Name).Bind(item, delegate (BinderOptions options)
                {
                    options.BindNonPublicProperties = true;
                });
                implementationInstance = AppSettingsHelper.SaveAppSettings(item, item.Name);
                services.AddSingleton(implementationInstance);
            }

            //services.AddScoped<IJwtTokenService, JwtTokenService>();
            IEngine engine = EngineContext.Create();
            engine.ConfigureServices(services, builder.Configuration);
        }

    }
}
