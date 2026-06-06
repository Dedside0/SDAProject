using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SDA.Db.Models;
using SDA.Db.Repositories;
using SDA.Web.Models;
using SDA.Web.Models.DTO;
using System.Security.Claims;
using System.Text.Json;

namespace SDAProject.Controllers
{
    [Authorize]
    public class ExamController(
        ITicketRepository ticketRepository,
        IQuestionRepository questionRepository,
        IUserRepository userRepository,
        IQuestionAttemptRepository questionAttemptRepository,
        IExamAttemptRepository examAttemptRepository,
        IUserMistakeQueueRepository userMistakeQueueRepository) : Controller
    {
        public class FinishExamDto
        {
            public Guid TicketId { get; set; }
            public int TimeSpentSeconds { get; set; }
            public string QuestionAttemptsJson { get; set; } = "";
        }


        public class QuestionAttemptDto
        {
            public Guid QuestionId { get; set; }
            public Guid? SelectedAnswerId { get; set; }
            public bool IsCorrect { get; set; }
            public string? Topic { get; set; }
            public DateTime AnsweredAt { get; set; }
            public int TimeSpentSeconds { get; set; }
        }

        public class CheckQuestionDto
        {
            public Guid QuestionId { get; set; }
            public Guid AnswerId { get; set; }
        }


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
                        Exploration = tQuest.Question.Exploration,
                        Topic = tQuest.Question.Topic?.Name,
                        Answers = tQuest.Question.Answers.Select(ans => new AnswerVm()
                        {
                            Id = ans.Id,
                            Text = ans.Text,
                            Order = ans.Order
                        }).ToList()
                    }
                }).ToList()

            };
            return View(ticketVm);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CheckQuestion([FromBody] CheckAnswerDTO dto)
        {
            if (dto is null)
                return BadRequest();

            var question = questionRepository.GetById(dto.QuestionId);
            if (question is null)
                return BadRequest();

            var rightAnswer = question.Answers.FirstOrDefault(x => x.IsRight);
            if (rightAnswer is null)
                return BadRequest();

            return Ok(new { correct = rightAnswer.Id == dto.AnswerId, correctId=rightAnswer.Id });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishExam(FinishExamDto dto)
        {
            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var qaList = new List<QuestionAttemptDto>();
            if (!string.IsNullOrWhiteSpace(dto.QuestionAttemptsJson))
            {
                qaList = JsonSerializer.Deserialize<List<QuestionAttemptDto>>(
                    dto.QuestionAttemptsJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new();
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


    }
}
