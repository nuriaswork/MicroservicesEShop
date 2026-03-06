using MsEShop.Web.Constants;
using MsEShop.Web.Models;
using MsEShop.Web.Service.IService;

namespace MsEShop.Web.Service
{
    public class AuthService : IAuthService
    {
        private const string ApiControllerName = "/api/auth";
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.AuthAPIBase + ApiControllerName + "/AssignRole",
                ApiType = Enums.ApiType.POST,
                Data = registrationRequestDto
            }, true);
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.AuthAPIBase + ApiControllerName + "/login",
                ApiType = Enums.ApiType.POST,
                Data = loginRequestDto
            }, false);
        }

        public async Task<ResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.AuthAPIBase + ApiControllerName + "/register",
                ApiType = Enums.ApiType.POST,
                Data = registrationRequestDto
            }, false);
        }
    }
}
