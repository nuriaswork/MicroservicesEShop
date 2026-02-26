using Microsoft.AspNetCore.Mvc;
using MsEShop.Web.Models;
using MsEShop.Web.Service.IService;
using Newtonsoft.Json;

namespace MsEShop.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto> list = [];

            ResponseDto response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.Success)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }
    }
}
