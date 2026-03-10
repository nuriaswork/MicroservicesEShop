using MsEShop.Services.ShoppingCartAPI.Models.Dto;
using MsEShop.Services.ShoppingCartAPI.Services.IServices;
using Newtonsoft.Json;

namespace MsEShop.Services.ShoppingCartAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> GetCouponAsync(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("CoupontHttpClient");
            var responseMessage = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");
            string content = await responseMessage.Content.ReadAsStringAsync();
            ResponseDto responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto != null && responseDto.Success)
            {
                return JsonConvert.DeserializeObject<CouponDto>(responseDto.Result.ToString());
            }
            return new CouponDto();
        }
    }
}
