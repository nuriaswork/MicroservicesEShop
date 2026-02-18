using AutoMapper;
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
                _response.Result = _mapper.Map<IEnumerable<Coupon>>(coupons);
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
        public ResponseDto GetByCode(string code) {
            try
            {
                Coupon coupon = _db.Coupons.First(c => c.Code == code);
                _response.Result= _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception e)
            {
                _response.Success= false;
                _response.Message = e.Message;
            }
            return _response;
        }
    }
}
