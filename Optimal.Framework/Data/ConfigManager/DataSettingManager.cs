using Microsoft.Extensions.Configuration;
using Optimal.Framework.Infrastructure;

namespace Optimal.Framework.Data.ConfigManager
{
    public static class DataSettingManager
    {
        public static DataConfig LoadSettings(IConfiguration configuration)
        {
            if (Singleton<DataConfig>.Instance == null)
                Singleton<DataConfig>.Instance = new DataConfig();

            var connectionStringsSection = configuration.GetConnectionString();

            if (connectionStringsSection != null)
            {
                Singleton<DataConfig>.Instance = new DataConfig();
                configuration.Bind("ConnectionStrings", Singleton<DataConfig>.Instance);
            }

            return Singleton<DataConfig>.Instance;
        }

        public static IConfigurationSection GetConnectionString(this IConfiguration configuration)
        {
            return configuration?.GetSection("ConnectionStrings");
        }
    }

}
