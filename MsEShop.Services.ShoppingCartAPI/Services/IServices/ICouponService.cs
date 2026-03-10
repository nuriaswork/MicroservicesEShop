using MsEShop.Services.ShoppingCartAPI.Models.Dto;

namespace MsEShop.Services.ShoppingCartAPI.Services.IServices
{
    public interface ICouponService
    {
        public Task<CouponDto> GetCouponAsync(string couponCode);
    }
}
