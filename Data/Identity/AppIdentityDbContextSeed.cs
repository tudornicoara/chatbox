using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBox.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ChatBox.Data.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Bob",
                        Email = "bob@test.com",
                        UserName = "bob@test.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Ted",
                        Email = "ted@test.com",
                        UserName = "ted@test.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Admin",
                        Email = "admin@test.com",
                        UserName = "admin@test.com"
                    }
                };
                
                var roles = new List<AppRole>
                {
                    new AppRole {Name = "Admin"},
                    new AppRole {Name = "Member"}
                };
                
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(user, "Member");
                    if (user.Email == "admin@test.com")
                        await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}