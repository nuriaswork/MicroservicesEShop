using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MsEShop.Web.Constants;
using MsEShop.Web.Models;
using MsEShop.Web.Service.IService;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MsEShop.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IJwtTokenLoader _jwtTokenLoader;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider, IJwtTokenLoader jwtTokenLoader)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
            _jwtTokenLoader = jwtTokenLoader;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            ResponseDto responseDto = await _authService.LoginAsync(model);
            if (responseDto != null && responseDto.Success)
            {
                LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                await SignInUer(loginResponse.Token);

                _tokenProvider.SetToken(loginResponse.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                //as we have asp-validation-summary component in Login view, we can use this:
                ModelState.AddModelError("CustomError", responseDto.Message);
                TempData["error"] = responseDto.Message;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new (Roles.Admin, Roles.Admin),
                new (Roles.Customer, Roles.Customer)
            };
            ViewBag.RoleList = roleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto result = await _authService.RegisterAsync(model);

                if (result != null && result.Success)
                {
                    if (string.IsNullOrEmpty(model.Role)) model.Role = Roles.Customer;

                    result = await _authService.AssignRoleAsync(model);

                    if (result != null && result.Success)
                    {
                        return RedirectToAction(nameof(Login));
                    }
                    else
                    {
                        TempData["error"] = "Error assigning role to user. " + result?.Message;
                    }
                }
                else
                {
                    TempData["error"] = "Error creating user. " + result?.Message;
                }
            }
            var roleList = new List<SelectListItem>()            {
                new (Roles.Admin, Roles.Admin),
                new (Roles.Customer, Roles.Customer)
            };
            ViewBag.RoleList = roleList;
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            _tokenProvider.ClearToken();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUer(string token)
        {
            //this logic is for User.Identity.IsAuthenticated to work: we need to inform .NET Identity how a user isAuthenticated
            var principal = new ClaimsPrincipal(_jwtTokenLoader.ReadClaimsFromJwtToken(token));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

    }
}
