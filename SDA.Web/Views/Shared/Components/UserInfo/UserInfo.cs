using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SDA.Db.Models;
using SDA.Web.Models.DTO;
using SDA.Db.Repositories;

namespace SDA.Web.Views.Shared.Components.UserInfo
{
    public class UserInfoViewComponent(IUserMistakeQueueRepository userMistakeQueueRepository,
        IQuestionAttemptRepository questionAttemptRepository,
     UserManager<User> userManager) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdString = userManager.GetUserId(UserClaimsPrincipal);

            var userId = Guid.Parse(userIdString);

            var allAttempts = questionAttemptRepository.GetUserAttempts(userId);

            var questionCount = allAttempts.Count();
            var rightPercent = questionCount==0? 0 : (int)(100*((double)allAttempts.Count(x => x.IsCorrect) / questionCount));

            var info = new UserInfoDto { Questions = questionCount, RightPercent = rightPercent };
            return View("UserInfo", info);
        }
    }
}
