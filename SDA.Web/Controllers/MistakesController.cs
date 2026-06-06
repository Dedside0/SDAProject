using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SDA.Db.Models;
using SDA.Db.Repositories;
using System.Security.Claims;
using System.Text.Json;

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

                    if (qst is not null)
                        questions.Add(qst);
                }
                return View(questions);
            }
            catch
            {
                return View();
            }

        }

        public class UserMistakeDto
        {
            public Guid questionId { get; set; }
            public Guid selectedAnswerId { get; set; }
            public bool isCorrect { get; set; }
            public DateTime answeredAt { get; set; }
            public int timeSpentSeconds { get; set; }
        }

        public class FinishReviewModel
        {
            public string QuestionAttemptsJson { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishReview(FinishReviewModel model)
        {
            if (string.IsNullOrEmpty(model.QuestionAttemptsJson))
            {
                TempData["Error"] = "Нет данных для сохранения";
                return RedirectToAction("Index");
            }

            var dto = JsonSerializer.Deserialize<List<UserMistakeDto>>(model.QuestionAttemptsJson);

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            foreach (var attempt in dto)
            {
                var mistake = userMistakeQueueRepository.GetUserMistake(userId, attempt.questionId);
                if (mistake == null) continue;

                if (attempt.isCorrect)
                {
                    userMistakeQueueRepository.SetAsMastered(mistake.Id);
                }
            }

            TempData["Success"] = "Работа над ошибками завершена!";
            return RedirectToAction("Stats","Home");
        }
    }
}
