using MsEShop.Web.Models;

namespace MsEShop.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto> GetCartByUserIdAsync(string userId);
        Task<ResponseDto> UpsertCartAsync(CartRequestDto cartRequestDto);
        Task<ResponseDto> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDto> ApplyCouponAsync(CartRequestDto cartRequestDto);
        Task<ResponseDto> RemoveCouponAsync(CartRequestDto cartRequestDto);


    }
}
