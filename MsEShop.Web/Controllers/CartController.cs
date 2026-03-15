using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;
using MsEShop.Web.Models;
using MsEShop.Web.Service.IService;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MsEShop.Web.Controllers
{
    public class CartController : BaseLoggedController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedUser());
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            CartDto cartDto = await LoadCartDtoBasedOnLoggedUser();
            OrderDto orderDto = new()
            {
                Cart = cartDto
            };
            return View(orderDto);
        }

        public async Task<IActionResult> RemoveProduct(int cartDetailsId)
        {
            var responseDto = await _cartService.RemoveFromCartAsync(cartDetailsId);
            if (responseDto != null && responseDto.Success)
            {
                TempData["sucess"] = "Product removed from cart";
            }
            else
            {
                TempData["error"] = "Error removing product from cart";
            }
            return RedirectToAction(nameof(CartIndex));

        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto model)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(model.CartHeader.CouponCode))
            {
                model.CartHeader.UserId = LoggedUserId;
                try
                {
                    CartRequestDto cartRequestDto = new()
                    {
                        CartHeader = model.CartHeader,
                    };

                    ResponseDto responseDto = new();

                    responseDto = await _cartService.ApplyCouponAsync(cartRequestDto);

                    if (responseDto != null && responseDto.Success)
                    {
                        TempData["sucess"] = "Coupon applied";
                    }
                }
                catch (Exception e)
                {
                    TempData["error"] = e.Message + e.InnerException?.Message;
                }
            }
            else
            {
                TempData["error"] = "Errors in form";
            }
            return RedirectToAction(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CartRequestDto cartRequestDto = new()
                    {
                        CartHeader = new()
                        {
                            UserId = LoggedUserId,
                            CouponCode = string.Empty
                        }
                    };

                    ResponseDto responseDto = new();

                    responseDto = await _cartService.RemoveCouponAsync(cartRequestDto);

                    if (responseDto != null && responseDto.Success)
                    {
                        TempData["sucess"] = "Coupon removed";
                    }
                }
                catch (Exception e)
                {
                    TempData["error"] = e.Message + e.InnerException?.Message;
                }
            }
            else
            {
                TempData["error"] = "Errors in form";
            }
            return RedirectToAction(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart()
        {
            EmailCartDto emailCartDto = new();
            try
            {
                CartDto cartFromDB = await LoadCartDtoBasedOnLoggedUser();
                if (cartFromDB == null || cartFromDB.CartDetails == null || cartFromDB.CartDetails.Count() <= 0)
                {
                    TempData["error"] = "Cart is empty, email cannot be sent. ";
                }
                else
                {
                    emailCartDto.UserEmail = LoggedUserEmail;
                    emailCartDto.CartHeader = cartFromDB.CartHeader;
                    emailCartDto.CartDetails = cartFromDB.CartDetails;

                    var responseDto = await _cartService.EmailCart(emailCartDto);

                    if (responseDto != null && responseDto.Success)
                    {
                        TempData["success"] = "Email is being processed and sent. You can continue shopping :)";
                    }
                    else
                    {
                        TempData["error"] = "Sorry, email cannot be sent. " + responseDto.Message;
                    }
                }
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message + e.InnerException?.Message;
            }
            return RedirectToAction(nameof(CartIndex));
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedUser()
        {
            var userId = LoggedUserId;
            var cartResponse = await _cartService.GetCartByUserIdAsync(userId);
            if (cartResponse != null && cartResponse.Success)
            {
                var cartDto = JsonConvert.DeserializeObject<CartDto>(cartResponse.Result.ToString());
                return cartDto;
            }
            return new CartDto();
        }
    }
}
