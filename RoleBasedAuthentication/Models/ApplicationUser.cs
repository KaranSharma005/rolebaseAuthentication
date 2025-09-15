using Microsoft.AspNetCore.Identity;

namespace RoleBasedAuthentication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public bool status { get; set; } = true;
    }
}
