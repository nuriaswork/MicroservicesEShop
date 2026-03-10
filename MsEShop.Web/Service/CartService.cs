using MsEShop.Web.Constants;
using MsEShop.Web.Models;
using MsEShop.Web.Service.IService;

namespace MsEShop.Web.Service
{
    public class CartService : ICartService
    {
        private const string ApiControllerName = "/api/cart";
        private readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> GetCartByUserIdAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.ShoppingCartAPIBase + ApiControllerName + $"/GetCart/{userId}",
                ApiType = Enums.ApiType.GET
            }, true);
        }

        public async Task<ResponseDto> UpsertCartAsync(CartRequestDto cartRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.ShoppingCartAPIBase + ApiControllerName + "/CartUpsert",
                ApiType = Enums.ApiType.POST,
                Data = cartRequestDto
            }, true);
        }

        public async Task<ResponseDto> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.ShoppingCartAPIBase + ApiControllerName + "/CartDetailsRemove",
                ApiType = Enums.ApiType.POST,
                Data = cartDetailsId
            }, true);
        }

        public async Task<ResponseDto> ApplyCouponAsync(CartRequestDto cartRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.ShoppingCartAPIBase + ApiControllerName + "/ApplyCoupon",
                ApiType = Enums.ApiType.POST,
                Data = cartRequestDto
            }, true);
        }

        public async Task<ResponseDto> RemoveCouponAsync(CartRequestDto cartRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Uri = ApisUri.ShoppingCartAPIBase + ApiControllerName + "/RemoveCoupon",
                ApiType = Enums.ApiType.POST,
                Data= cartRequestDto
            }, true);
        }

    }
}
