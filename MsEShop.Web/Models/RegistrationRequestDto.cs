using System.ComponentModel.DataAnnotations;

namespace MsEShop.Web.Models
{
    public class RegistrationRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]

        public string Name { get; set; }

        public string PhoneNumber { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        public string Role { get; set; }
    }
}
