using MsEShop.Services.EmailAPI.Models.Dto;

namespace MsEShop.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(EmailCartDto emailCartDto);
    }
}
