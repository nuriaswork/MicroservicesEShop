using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MsEShop.Services.ShoppingCartAPI.Data;
using MsEShop.Services.ShoppingCartAPI.Models;
using MsEShop.Services.ShoppingCartAPI.Models.Dto;
using MsEShop.Services.ShoppingCartAPI.Services.IServices;
using System.Reflection.PortableExecutable;

namespace MsEShop.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private ResponseDto _responseDto;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        public CartAPIController(IMapper mapper, AppDbContext db, IProductService productService, ICouponService couponService)
        {
            _mapper = mapper;
            _db = db;
            _responseDto = new ResponseDto() { Success = true };
            _productService = productService;
            _couponService = couponService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_db.CartHeaders.First(ch => ch.UserId == userId))
                };
                cartDto.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(
                    _db.CartDetails.Where(cd => cd.CartHeaderId == cartDto.CartHeader.CartHeaderId));

                IEnumerable<ProductDto> products = await _productService.GetProductsAsync();

                foreach (var item in cartDto.CartDetails)
                {
                    item.productDto = products.First(p => p.ProductId == item.ProductId);
                    cartDto.CartHeader.CartTotal += item.Count * item.productDto.Price;
                }

                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    var couponDto = await _couponService.GetCouponAsync(cartDto.CartHeader.CouponCode);
                    if (couponDto != null && cartDto.CartHeader.CartTotal >= couponDto.MinAmount)
                    {
                        cartDto.CartHeader.CartTotal -= couponDto.DiscountAmount;
                        cartDto.CartHeader.Discount = couponDto.DiscountAmount;
                    }
                }

                _responseDto.Result = cartDto;

            }
            catch (Exception e)
            {
                _responseDto.Success = false;
                _responseDto.Message = e.Message;
            }

            return _responseDto;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert([FromBody] CartRequestDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();
                    cartDto.CartDetails.CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    CartDetails cartDetailsFromDb = await _db.CartDetails.FirstOrDefaultAsync(cd =>
                                                                            cd.CartHeaderId == cartHeaderFromDb.CartHeaderId &&
                                                                            cd.ProductId == cartDto.CartDetails.ProductId);
                    if (cartDetailsFromDb == null)
                    {
                        cartDto.CartDetails.CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        cartDetailsFromDb.Count = cartDto.CartDetails.Count;
                        _db.CartDetails.Update(cartDetailsFromDb);
                        await _db.SaveChangesAsync();
                    }
                }
                _responseDto.Result = cartDto;
            }
            catch (Exception e)
            {
                _responseDto.Success = false;
                _responseDto.Message = e.Message;
                if (e.InnerException != null) _responseDto.Message += e.InnerException.Message;
            }

            return _responseDto;
        }

        [HttpPost("CartDetailsRemove")]
        public async Task<ResponseDto> CartDetailsRemove([FromBody] int cartDetailsId)
        {
            CartDetails cartDetailsFromDB = await _db.CartDetails.FindAsync(cartDetailsId);
            if (cartDetailsFromDB != null && cartDetailsFromDB.CartDetailsId == cartDetailsId)
            {
                int totalDetailsCount = _db.CartDetails.Count(cd => cd.CartHeaderId == cartDetailsFromDB.CartHeaderId);
                _db.CartDetails.Remove(cartDetailsFromDB);
                if (totalDetailsCount == 1)
                {
                    var cartHeaerFromDb = await _db.CartHeaders.FindAsync(cartDetailsFromDB.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeaerFromDb);
                }
                await _db.SaveChangesAsync();

                return _responseDto;
            }
            else
            {
                _responseDto.Success = false;
                _responseDto.Message = "No such Cart Detail.";
            }
            return _responseDto;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CartRequestDto cartRequestDto)
        {
            try
            {
                CartHeader cartHeaderFromDb = await _db.CartHeaders.FirstAsync(ch => ch.UserId == cartRequestDto.CartHeader.UserId);
                cartHeaderFromDb.CouponCode = cartRequestDto.CartHeader.CouponCode;
                _db.CartHeaders.Update(cartHeaderFromDb);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _responseDto.Success = false;
                _responseDto.Message = e.Message;
            }
            return _responseDto;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon([FromBody] CartRequestDto cartRequestDto)
        {
            try
            {
                CartHeader cartHeaderFromDb = await _db.CartHeaders.FirstAsync(ch => ch.UserId == cartRequestDto.CartHeader.UserId);
                cartHeaderFromDb.CouponCode = string.Empty;
                _db.CartHeaders.Update(cartHeaderFromDb);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _responseDto.Success = false;
                _responseDto.Message = e.Message;
            }
            return _responseDto;
        }
    }
}