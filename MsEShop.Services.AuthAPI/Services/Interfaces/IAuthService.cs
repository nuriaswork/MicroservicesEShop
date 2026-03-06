using MsEShop.Services.AuthAPI.Models.Dto;

namespace MsEShop.Services.AuthAPI.Services.Interfaces
{
    /// <summary>
    /// Interface for Authentication Services
    /// </summary>
    public interface IAuthService
    {

        /// <summary>
        /// Creates a user. 
        /// </summary>
        /// <returns>Returns "" if the user was created. Returns the error message if couldn't create the user.</returns>
        Task<string> Register(RegistrationRequestDto registrationRequestDto);

        /// <summary>
        /// Try to Login the user with the password provided.
        /// </summary>
        /// <returns>User and Token</returns>
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        /// <summary>
        /// Assigns role to user.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns>True if the role was assigned to user. False otherwise.</returns>
        Task<bool> AssignRole(string email, string roleName);
    }
}
