using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MsEShop.Services.AuthAPI.Data;
using MsEShop.Services.AuthAPI.Models;
using MsEShop.Services.AuthAPI.Models.Dto;
using MsEShop.Services.AuthAPI.Services.Interfaces;
using System.Diagnostics.Eventing.Reader;

namespace MsEShop.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private IMapper _mapper;

        //These 2 are automatically injected by .Net Identity, so we don't need to configure in Program.cs
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /// <summary>
        /// Try to Login the user with the password provided.
        /// </summary>
        /// <param name="login"></param>
        /// <returns>User and Token</returns>
        public async Task<LoginResponseDto> Login(LoginRequestDto login)
        {
            LoginResponseDto response = new() { User = null, Token = "" };

            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == login.UserName.ToLower());
            if (user == null)
                return response;

            bool isValidPassword = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!isValidPassword)
                return response;

            response.User = _mapper.Map<UserDto>(user);
            response.Token = _jwtTokenGenerator.GenerateToken(user); ;

            return response;

        }

        /// <summary>
        /// Creates a user. 
        /// </summary>
        /// <param name="registration"></param>
        /// <returns>Returns "" if the user was created. Returns the error message if couldn't create the user.</returns>
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

        /// <summary>
        /// Assigns role to user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns>True if the role was assigned to user. False otherwise.</returns>
        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
            if (user == null)
                return false;

            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())  //instead of await _roleManager.RoleExistsAsync(roleName)
            {
                _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            }
            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }
    }
}
