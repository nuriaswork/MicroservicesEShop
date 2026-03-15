using System.ComponentModel.DataAnnotations;

namespace MsEShop.Web.Models
{
    public class OrderDto
    {
        public CartDto Cart { get; set; }

        [Required]
        public string Name { get; set; }
                
        [Required]
        [Phone]
        public string Phone { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
