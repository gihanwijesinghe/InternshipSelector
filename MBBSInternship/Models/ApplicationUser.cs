using Microsoft.AspNetCore.Identity;

namespace MBBSInternship.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
