namespace MsEShop.Web.Service.IService
{
    //As we can implement the token management in different ways (cookie, session, etc.) we'll create an Interface
    public interface ITokenProvider
    {
        void SetToken(string token);
        string GetToken();
        void ClearToken();

    }
}
