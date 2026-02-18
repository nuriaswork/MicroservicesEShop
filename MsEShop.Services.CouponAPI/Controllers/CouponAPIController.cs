using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MsEShop.Services.CouponAPI.Data;
using MsEShop.Services.CouponAPI.Models;

namespace MsEShop.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CouponAPIController(AppDbContext db) => _db = db;

        [HttpGet]
        public object GetAll()
        {
            try
            {
                IEnumerable<Coupon> coupons = _db.Coupons.ToList();
                return coupons;
            }
            catch (Exception)
            {

            }
            return null;
        }

        [HttpGet]
        [Route("{id:int}")]
        public object Get(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.FirstOrDefault(c => c.Id == id);
                return coupon;
            }
            catch (Exception)
            {

            }
            return null;
        }

    }
}
