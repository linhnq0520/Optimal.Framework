using Microsoft.IdentityModel.Tokens;
using Optimal.Framework.Configuration;
using Optimal.Framework.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Optimal.Framework.Services
{
    public class JwtTokenService:IJwtTokenService
    {
        private readonly WebApiSettings _webApiSetting;
        public JwtTokenService(WebApiSettings webApiSetting)
        {
            _webApiSetting = webApiSetting;
        }
        public virtual string GetNewJwtToken(User user, long expireSeconds = 0L)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            long num = now.AddDays(_webApiSetting.TokenLifetimeDays).ToUnixTimeSeconds();
            if (expireSeconds > 0)
            {
                num = expireSeconds;
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim("nbf", now.ToUnixTimeSeconds().ToString()),
                new Claim("exp", num.ToString()),
                new Claim("Id", user.Id.ToString()),
                new Claim(WebApiCommonDefaults.LoginName, user.LoginName),
                new Claim(WebApiCommonDefaults.UserName, user.Username),
                new Claim(WebApiCommonDefaults.UserCode, user.UserCode)
            };
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] bytes = Encoding.UTF8.GetBytes(_webApiSetting.SecretKey);
            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(bytes), SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                new JwtHeader(signingCredentials),
                new JwtPayload(claims));
            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
