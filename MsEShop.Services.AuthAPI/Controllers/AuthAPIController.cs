using Microsoft.AspNetCore.Mvc;
using MsEShop.Services.AuthAPI.Models.Dto;
using MsEShop.Services.AuthAPI.Services.Interfaces;

namespace MsEShop.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private IAuthService _authService;
        protected ResponseDto _response;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new() { Success = true };
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var response = await _authService.Register(model);
            if (!string.IsNullOrEmpty(response))
            {
                _response.Success = false;
                _response.Message = response;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            LoginResponseDto result = await _authService.Login(model);
            if (result.User == null)
            {
                _response.Success = false;
                _response.Message = "Username or Password is incorrect.";
                return BadRequest(_response);
            }
            else
            {
                _response.Result = result;
                return Ok(_response);
            }
        }


        /// <summary>
        /// Assigns role to user.
        /// </summary>
        /// <returns>True if the role was assigned to user. False otherwise.</returns>
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var result = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!result)
            {
                _response.Success = false;
                _response.Message = "Error assigning role to user.";
                return BadRequest(_response);
            }
            return Ok(_response);
        }
    }
}
