using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MsEShop.Services.CouponAPI.Data;
using MsEShop.Services.CouponAPI.Models;
using MsEShop.Services.CouponAPI.Models.Dto;

namespace MsEShop.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;

        public CouponAPIController(AppDbContext db)
        {
            _db = db;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto GetAll()
        {
            try
            {
                IEnumerable<Coupon> coupons = _db.Coupons.ToList();
                _response.Result = coupons;
            }
            catch (Exception e)
            {
                _response.Success = false;
                _response.Message = e.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.First(c => c.Id == id);
                _response.Result = coupon;
            }
            catch (Exception e)
            {
                _response.Success = false;
                _response.Message = e.Message;
            }
            return _response;
        }

    }
}
