using Optimal.Framework.Data;

namespace Optimal.Framework.Domain
{
    public class User : BaseEntity
    {
        public string UserCode { get; set; }

        public string LoginName { get; set; }

        public string Username { get; set; }
    }
}
