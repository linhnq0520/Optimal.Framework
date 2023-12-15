using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimal.Framework.Configuration
{
    public class WebApiSettings : ISetting
    {
        /// <summary>
        /// Developer mode
        /// </summary>
        /// <value></value>
        public bool DeveloperMode { get; set; }

        /// <summary>
        /// Secret key
        /// </summary>
        /// <value></value>
        public string SecretKey { get; set; }

        /// <summary>
        /// Token life time in days
        /// </summary>
        /// <value></value>
        public int TokenLifetimeDays { get; set; }

        /// <summary>
        /// Time buffer (in milliseconds)
        /// </summary>
        public long BufferTime { get; set; }
    }
}
