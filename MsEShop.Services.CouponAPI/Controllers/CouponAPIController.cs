using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MsEShop.Services.CouponAPI.Data;
using MsEShop.Services.CouponAPI.Models;
using MsEShop.Services.CouponAPI.Models.Dto;

namespace MsEShop.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto GetAll()
        {
            try
            {
                IEnumerable<Coupon> coupons = _db.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
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
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception e)
            {
                _response.Success = false;
                _response.Message = e.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon = _db.Coupons.First(c => c.Code == code);
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception e)
            {
                _response.Success = false;
                _response.Message = e.Message;
            }
            return _response;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                coupon.CreatedOn = DateTime.Now;
                coupon.LastUpdatedOn = DateTime.Now;
                _db.Coupons.Add(coupon);
                _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDto>(coupon);
                _response.Success = true;
                _response.Message = "Created sucesfully";
                return _response;
            }
            catch (Exception e)
            {
                _response.Success = false;
                _response.Message = "Error creating: " + e.Message;
            }
            return _response;
        }

        [HttpPut]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                coupon.LastUpdatedOn = DateTime.Now;
                _db.Coupons.Update(coupon);
                _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDto>(coupon);
                _response.Success = true;
                _response.Message = "Updated sucesfully";
                return _response;
            }
            catch (Exception e)
            {
                _response.Success = false;
                _response.Message = "Error updating: " + e.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon coupon = _db.Coupons.First(c => c.Id == id);
                _db.Coupons.Remove(coupon);
                _db.SaveChanges();
                _response.Result = _mapper.Map<CouponDto>(coupon);
                _response.Success = true;
                _response.Message = "Removed sucesfully";
                return _response;
            }
            catch (Exception e)
            {
                _response.Success = false;
                _response.Message = "Error removing: " + e.Message;
            }
            return _response;
        }
    }
}
