using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Optimal.Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimal.Framework.ConfigManager.DataConfig
{
    public static class DataSettingManager
    {
        public static DataConfig LoadSettings(IConfiguration configuration)
        {
            if (Singleton<DataConfig>.Instance == null) Singleton<DataConfig>.Instance = new DataConfig();
            var conn = configuration.GetConnectionString("ConnectionString");
            Singleton<DataConfig>.Instance.ConnectionString = configuration.GetConnectionString("ConnectionString");
            return Singleton<DataConfig>.Instance;
        }
    }
}
