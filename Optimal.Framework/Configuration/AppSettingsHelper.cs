using Newtonsoft.Json.Linq;
using Optimal.Framework.Helper;
using Optimal.Framework.Infrastructure;

namespace Optimal.Framework.Configuration
{
    internal class AppSettingsHelper
    {
        public static AppSettings SaveAppSettings(IConfig configurations, string configName)
        {
            ArgumentNullException.ThrowIfNull(configurations);

            AppSettings appSettings = Singleton<AppSettings>.Instance ?? new AppSettings();
            Singleton<AppSettings>.Instance = appSettings;

            appSettings.Configuration[configName] = configurations.ToObject<JToken>();

            return appSettings;
        }
    }
}
