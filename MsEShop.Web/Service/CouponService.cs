using MsEShop.Web.Constants;
using MsEShop.Web.Models;
using MsEShop.Web.Service.IService;

namespace MsEShop.Web.Service
{
    public class CouponService : ICouponService
    {
        private const string ApiControllerName = "/api/coupon";
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> CreateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.CouponAPIBase + ApiControllerName,
                ApiType = Enums.ApiType.POST,
                Data = couponDto
            }, true);
        }

        public async Task<ResponseDto> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.CouponAPIBase + ApiControllerName + $"/{id}",
                ApiType = Enums.ApiType.DELETE
            }, true);
        }

        public async Task<ResponseDto> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.CouponAPIBase + ApiControllerName,
                ApiType = Enums.ApiType.GET
            }, true);
        }

        public async Task<ResponseDto> GetCouponAsync(string code)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.CouponAPIBase + ApiControllerName + $"/GetByCode/{code}",
                ApiType = Enums.ApiType.GET
            }, true);
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.CouponAPIBase + ApiControllerName + $"/{id}",
                ApiType = Enums.ApiType.GET
            }, true);
        }

        public async Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto)
        {

            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.CouponAPIBase + ApiControllerName,
                ApiType = Enums.ApiType.PUT,
                Data = couponDto
            }, true);
        }
    }
}
