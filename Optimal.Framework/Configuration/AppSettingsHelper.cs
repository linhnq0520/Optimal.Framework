using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Optimal.Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optimal.Framework.Helper;

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
