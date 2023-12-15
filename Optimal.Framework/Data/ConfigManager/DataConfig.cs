using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimal.Framework.ConfigManager.DataConfig
{
    public class DataConfig
    {
        public string ConnectionString { get; set; }
        public int? SQLCommandTimeout { get; set; }
    }
}
