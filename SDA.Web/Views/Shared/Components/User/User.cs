using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SDA.Db.Models;
using SDA.Web.Models.DTO;
using SDA.Db.Repositories;

namespace SDA.Web.Views.Shared.Components.UserInfo
{
    public class UserViewComponent(
     UserManager<User> userManager) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId= userManager.GetUserId(UserClaimsPrincipal);
                var user = await userManager.FindByIdAsync(userId);
                return View("User", user);
            }
            else
            {
                return View("User");
            }

        }
    }
}
