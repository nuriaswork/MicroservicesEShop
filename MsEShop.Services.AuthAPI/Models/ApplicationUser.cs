using Microsoft.AspNetCore.Identity;

namespace MsEShop.Services.AuthAPI.Models
{
    //as this class is child of IdentityUser, a new table won't be created. The properties will be added to aspnetusers Net Identity table
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
