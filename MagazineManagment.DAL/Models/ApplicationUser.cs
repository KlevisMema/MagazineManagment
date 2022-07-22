using Microsoft.AspNetCore.Identity;

namespace MagazineManagment.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public int  Age { get; set; }
    }
}
