using Microsoft.AspNetCore.Identity;

namespace ChatBox.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
