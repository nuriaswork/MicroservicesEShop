using MsEShop.Web.Service.IService;

namespace MsEShop.Web.Service
{
    public class TokenProviderService : ITokenProvider
    {
        //COOKIE MANAGEMENT: it's configured in Program.cs with builder.Services.AddHttpContextAccessor();
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProviderService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(Constants.CookieNames.AuthenticationTokenCookie);
        }

        public string GetToken()
        {
            string token = null;
            _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(Constants.CookieNames.AuthenticationTokenCookie, out token);
            return token ?? "";
        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(Constants.CookieNames.AuthenticationTokenCookie, token);
        }
    }
}
