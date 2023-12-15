using Microsoft.Extensions.Configuration;
using Optimal.Framework.Infrastructure;

namespace Optimal.Framework.Data.ConfigManager
{
    public static class DataSettingManager
    {
        public static DataConfig LoadSettings(IConfiguration configuration)
        {
            if (Singleton<DataConfig>.Instance == null) Singleton<DataConfig>.Instance = new DataConfig();
            Singleton<DataConfig>.Instance.ConnectionString = configuration.GetConnectionString("ConnectionString");
            return Singleton<DataConfig>.Instance;
        }
    }
}
