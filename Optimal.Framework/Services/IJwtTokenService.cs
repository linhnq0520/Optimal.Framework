using Optimal.Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimal.Framework.Services
{
    public interface IJwtTokenService
    {
        string GetNewJwtToken(User user, long expireSeconds = 0L);
    }
}
