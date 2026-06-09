using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SDA.Db.Models;
using SDA.Db.Repositories;
using SDA.Web;
using SDA.Web.Models;
using SDA.Web.Models.DTO;
using System.Security.Claims;
using System.Text.Json;

namespace SDAProject.Controllers
{
    [Authorize]
    public partial class ExamController(
        ITicketRepository ticketRepository,
        IQuestionRepository questionRepository,
        IQuestionAttemptRepository questionAttemptRepository,
        IExamAttemptRepository examAttemptRepository,
        IUserMistakeQueueRepository userMistakeQueueRepository,
        UserManager<User> userManager) : Controller
    {
        public async Task<IActionResult> Index(Guid id)
        {
            var ticket = await ticketRepository.GetById(id);
            if (ticket is null)
                return BadRequest();

            var ticketVm = new TicketVm()
            {
                Id = ticket.Id,
                Name = ticket.Name,
                TicketQuestions = ticket.TicketQuestions.Select(tQuest => new TicketQuestionVm()
                {
                    Order = tQuest.Order,
                    Question = new QuestionVm()
                    {
                        Id = tQuest.QuestionId,
                        Text = tQuest.Question.Text,
                        ImageUrl = tQuest.Question.ImageUrl,
                        Explanation = tQuest.Question.Explanation,
                        Topic = tQuest.Question.Topic?.Name,
                        Answers = tQuest.Question.Answers.Select(ans => new AnswerVm()
                        {
                            Id = ans.Id,
                            Text = ans.Text,
                            Order = ans.Order,
                            IsRight = ans.IsRight,
                        }).ToList()
                    }
                }).ToList()

            };
            return View(ticketVm);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishExam(FinishExamDto dto)
        {
            var userId = Guid.Parse(userManager.GetUserId(User));

            var qaList = new List<QuestionAttemptDto>();
            if (!string.IsNullOrWhiteSpace(dto.QuestionAttemptsJson))
            {
                qaList = JsonSerializer.Deserialize<List<QuestionAttemptDto>>(
                    dto.QuestionAttemptsJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new();
            }
            foreach (var item in qaList)
            {
                var question = questionRepository.GetById(item.QuestionId);
                var correctAns = question.Answers.FirstOrDefault(a => a.IsRight);
                item.IsCorrect = correctAns.Id == item.SelectedAnswerId;
            }
            int correctCount = 0;
            int wrongCount = 0;

            foreach (var qa in qaList)
            {
                if (qa.SelectedAnswerId == null) continue; 
                if (qa.IsCorrect) correctCount++;
                else wrongCount++;
            }

            int totalQuestions = qaList.Count;

            bool isPassed = wrongCount <= 2;

            var attempt = new ExamAttempt
            {
                Id = Guid.NewGuid(),
                TicketId = dto.TicketId,
                UserId = userId,
                FinishedAt = DateTime.UtcNow,
                IsPassed = isPassed,
                TimeSpentSeconds = dto.TimeSpentSeconds,
                CorrectCount = correctCount,
                WrongCount = wrongCount,
                TotalQuestions = totalQuestions
            };

            examAttemptRepository.Create(attempt);

            foreach (var qa in qaList)
            {
                var qstAttempt = new QuestionAttempt
                {
                    Id = Guid.NewGuid(),
                    ExamAttemptId = attempt.Id,
                    QuestionId = qa.QuestionId,
                    SelectedAnswerId = qa.SelectedAnswerId,
                    IsCorrect = qa.IsCorrect,
                    Topic = qa.Topic,
                    UserId = userId,
                    AnsweredAt = qa.AnsweredAt.Kind == DateTimeKind.Utc
                                           ? qa.AnsweredAt
                                           : qa.AnsweredAt.ToUniversalTime()
                };
                questionAttemptRepository.Create(qstAttempt);
                if(qstAttempt.IsCorrect == false)
                {
                    userMistakeQueueRepository.Create(new UserMistakeQueue
                    {
                        AddedAt = qstAttempt.AnsweredAt,
                        IsMastered = false,
                        QuestionId = qa.QuestionId,
                        UserId = userId
                    });
                }
            }

            return RedirectToAction("Result", new { attemptId = attempt.Id });
        }


        [HttpGet]
        public async Task<IActionResult> Result(Guid attemptId)
        {
            var attempt = examAttemptRepository.GetById(attemptId);

            if (attempt == null) return NotFound();

            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (attempt.UserId != userId) return Forbid();

            var questionAttempts = questionAttemptRepository.GetListById(attemptId);

            var vm = ExamResultVm.Build(attempt, questionAttempts);
            return View(vm);
        }

        public IActionResult Traning()
        {
            var allQuestions = questionRepository.GetAll();

            var random = new Random();
            var shuffledQuestions = allQuestions.OrderBy(x => random.Next()).ToList();

            var vm = new TicketVm
            {
                Id = Guid.Empty,
                Name = "Режим тренировки: Все вопросы",
                TicketQuestions = shuffledQuestions.Select((q, index) => new TicketQuestionVm
                {
                    Order = index,
                    Question = q.ToQuestionVm()
                }).ToList()
            };

            return View(vm);
        }

    }
}
