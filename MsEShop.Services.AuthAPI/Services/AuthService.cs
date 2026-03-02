using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MsEShop.Services.AuthAPI.Data;
using MsEShop.Services.AuthAPI.Models;
using MsEShop.Services.AuthAPI.Models.Dto;
using MsEShop.Services.AuthAPI.Services.Interfaces;

namespace MsEShop.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;

        //These 2 are automatically injected by .Net Identity, so we don't need to configure in Program.cs
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<LoginResponseDto> Login(LoginRequestDto login)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Register(RegistrationRequestDto registration)
        {

            try
            {
                ApplicationUser user = new()
                {
                    UserName = registration.Email,
                    Email = registration.Email,
                    NormalizedEmail = registration.Email.ToUpper(),
                    PhoneNumber = registration.PhoneNumber,
                    Name = registration.Name
                };

                var result = await _userManager.CreateAsync(user, registration.Password);
                if (result.Succeeded)
                {
                    //check the user in the ddbb
                    ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registration.Email);
                    if (applicationUser != null && applicationUser.Email == registration.Email)
                    {
                        return "";
                    }
                    return "Unexpected Error on Registration.";
                }
                else
                {
                    return string.Join(",", result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
