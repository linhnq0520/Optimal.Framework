using Optimal.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimal.Framework.Services.Configuration
{
    public interface ISettingService
    {
        Task<ISetting> LoadSetting(Type type);
    }
}
