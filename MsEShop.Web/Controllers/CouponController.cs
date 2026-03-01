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
            else
            {
                TempData["error"] = response.Message;
            }

            return View(list);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto response = await _couponService.CreateCouponAsync(model);

                if (response != null && response.Success)
                {
                    TempData["success"] = "Coupon created!!";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response.Message;
                }

            }
            return View(model);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDto responseDto = await _couponService.GetCouponByIdAsync(couponId);

            if (responseDto != null && responseDto.Success)
            {
                CouponDto couponDto = JsonConvert.DeserializeObject<CouponDto>(responseDto.Result.ToString());
                return View(couponDto);
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            ResponseDto responseDto = await _couponService.DeleteCouponAsync(couponDto.Id);

            if (responseDto != null && responseDto.Success)
            {
                TempData["success"] = "Coupon deleted!!";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }

            return View(couponDto);
        }

        //We are not implementing Update functionallity
    }
}
