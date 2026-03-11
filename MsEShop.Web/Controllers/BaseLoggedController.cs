using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace MsEShop.Web.Controllers
{
    public class BaseLoggedController : Controller
    {
        public string LoggedUserId
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    return User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                }
                return string.Empty;
            }
        }


    }
}
