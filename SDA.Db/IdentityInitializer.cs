using Microsoft.AspNetCore.Identity;
using SDA.Db.Models;

namespace SDA.Db
{
    public class IdentityInitializer
    {
        public static void Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.FindByNameAsync(Constants.AdminRoleName).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(Constants.AdminRoleName)).Wait();
            }

            if (roleManager.FindByNameAsync(Constants.UserRoleName).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(Constants.UserRoleName)).Wait();
            }

            if (userManager.FindByNameAsync(Constants.DefaultAdminEmail).Result == null)
            {
                var admin = new User
                {
                    Email = Constants.DefaultAdminEmail,
                    UserName = Constants.DefaultAdminEmail,
                    //PhoneNumber = Constants.DefaultAdminPhone,
                    RegistrationDateTime = DateTime.UtcNow
                };

                var result = userManager.CreateAsync(admin, Constants.DefaultAdminPassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, Constants.AdminRoleName).Wait();
                }
            }
        }
    }
}
