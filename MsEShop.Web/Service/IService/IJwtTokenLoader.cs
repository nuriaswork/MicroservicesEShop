using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MsEShop.Web.Service.IService
{
    public interface IJwtTokenLoader
    {
        ClaimsIdentity ReadClaimsFromJwtToken(string jwtToken);
    }
}
