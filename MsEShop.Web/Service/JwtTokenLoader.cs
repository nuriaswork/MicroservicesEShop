using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using MsEShop.Web.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MsEShop.Web.Service
{
    public class JwtTokenLoader : IJwtTokenLoader
    {
        public ClaimsIdentity ReadClaimsFromJwtToken(string jwtToken)
        {
            JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name).Value));

            //this must be added to all .NET Identity claims: a claim with type ClaimTypes.Name
            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value));

            return identity;
        }
    }
}
