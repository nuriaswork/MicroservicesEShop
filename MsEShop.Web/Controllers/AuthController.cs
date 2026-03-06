using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MsEShop.Web.Constants;
using MsEShop.Web.Models;
using MsEShop.Web.Service.IService;

namespace MsEShop.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
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
        public IActionResult Logout()
        {
            return View();
        }

    }
}
