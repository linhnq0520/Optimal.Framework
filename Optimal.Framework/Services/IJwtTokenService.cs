using Optimal.Framework.Domain;

namespace Optimal.Framework.Services
{
    public interface IJwtTokenService
    {
        string GetNewJwtToken(User user, long expireSeconds = 0L);
    }
}
