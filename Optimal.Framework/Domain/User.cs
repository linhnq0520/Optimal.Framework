using Optimal.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimal.Framework.Domain
{
    public class User : BaseEntity
    {
        public string UserCode { get; set; }

        public string LoginName { get; set; }

        public string Username { get; set; }
    }
}
