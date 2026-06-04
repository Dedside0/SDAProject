using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SDA.Db.Models;
using SDA.Db.Repositories;

namespace SDA.Web.Controllers
{
    public class MistakesController(IUserMistakeQueueRepository userMistakeQueueRepository,
        IQuestionRepository questionRepository,
        UserManager<User> userManager) : Controller
    {
        public IActionResult Index()
        {
            try
            {
                var userid = Guid.Parse(userManager.GetUserId(User));
                var mistakes = userMistakeQueueRepository.GetAllByUserId(userid);
                var questions = new List<Question>();
                foreach (var item in mistakes)
                {
                    var qst = questionRepository.GetById(item.QuestionId);

                    if(qst is not null)
                        questions.Add(qst);
                }
                return View(questions);
            }
            catch
            {
                return View();
            }

        }
    }
}
