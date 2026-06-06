using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDA.Db;
using SDA.Db.Models;
using SDA.Db.Repositories;

namespace SDAProject.Controllers
{
    [Authorize]
    public class GeneratedExamController(IThemeRepository themeRepository,
        ITicketRepository ticketRepository,
        IQuestionRepository questionRepository,
        IExamAttemptRepository examAttemptRepository,
        IQuestionAttemptRepository questionAttemptRepository) : Controller
    {

        // Генерирует 20 вопросов (5 из каждой группы) и отдаёт View
        [HttpGet]
        public async Task<IActionResult> Start()
        {
            // Загружаем все темы с вопросами и ответами, сгруппированные по Group
            var topics = await themeRepository.GetQuestionsGrouped();

            var rng = new Random();

            // По 5 вопросов из каждой группы 1-4
            var selected = new List<GeneratedQuestion>();

            for (int group = 1; group <= 4; group++)
            {
                // Все вопросы группы из всех тем
                var groupQuestions = topics
                    .Where(t => t.Group == group)
                    .SelectMany(t => t.Questions.Select(q => new { q, t }))
                    .ToList();

                if (groupQuestions.Count < 5)
                {
                    // Если вопросов меньше 5 — берём сколько есть
                    foreach (var item in groupQuestions)
                        selected.Add(Map(item.q, item.t, group));
                }
                else
                {
                    foreach (var item in groupQuestions.OrderBy(_ => rng.Next()).Take(5))
                        selected.Add(Map(item.q, item.t, group));
                }
            }

            selected = selected.OrderBy(_ => rng.Next()).ToList();

            var vm = new GeneratedExamVm
            {
                Questions      = selected,
                InitialSeconds = 20 * 60
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckQuestion(
            [FromBody] CheckDto dto)
        {
            var answer =  questionRepository.GetById(dto.QuestionId).Answers.FirstOrDefault(x=>x.Id==dto.AnswerId);
            if (answer == null) return BadRequest("Ответ не найден");

            var question =  questionRepository.GetById(dto.QuestionId);
            var correct = question.Answers.FirstOrDefault(x => x.IsRight);

            return Ok(new { correct = answer.IsRight, correctId = correct?.Id });
        }

        public class CheckDto
        {
            public Guid QuestionId { get; set; }
            public Guid AnswerId   { get; set; }
        }

        // Принимает список групп у которых ровно 1 ошибка → возвращает доп. вопросы.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetExtraQuestions(
            [FromBody] ExtraRequestDto dto)
        {
            // dto.GroupsWithOneError  = [1, 3]   (группы, где ровно 1 ошибка)
            // dto.UsedQuestionIds     = [...] (уже показанные — не повторять)

            var rng = new Random();
            var usedIds = dto.UsedQuestionIds?.ToHashSet() ?? new();
            var result  = new List<GeneratedQuestion>();

            var topics = await themeRepository.GetAll();

            foreach (var group in dto.GroupsWithOneError)
            {
                var pool = topics
                    .Where(t => t.Group == group)
                    .SelectMany(t => t.Questions.Select(q => new { q, t }))
                    .Where(x => !usedIds.Contains(x.q.Id))
                    .ToList();

                // 5 доп. вопросов на каждую группу с одной ошибкой
                foreach (var item in pool.OrderBy(_ => rng.Next()).Take(5))
                    result.Add(Map(item.q, item.t, group));
            }

            return Ok(result);
        }

        public class ExtraRequestDto
        {
            public List<int>  GroupsWithOneError { get; set; } = new();
            public List<Guid> UsedQuestionIds   { get; set; } = new();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finish(FinishDto dto)
        {
            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var qaList = string.IsNullOrWhiteSpace(dto.QuestionAttemptsJson)
                ? new List<QaDto>()
                : JsonSerializer.Deserialize<List<QaDto>>(
                    dto.QuestionAttemptsJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                  ) ?? new();

            int correct = qaList.Count(x => x.IsCorrect);
            int wrong   = qaList.Count(x => !x.IsCorrect && x.SelectedAnswerId != null);

            var attempt = new ExamAttempt
            {
                Id               = Guid.NewGuid(),
                TicketId         = Guid.Empty,
                UserId           = userId,
                FinishedAt       = DateTime.UtcNow,
                IsPassed         = dto.IsPassed,
                TimeSpentSeconds = dto.TimeSpentSeconds,
                CorrectCount     = correct,
                WrongCount       = wrong,
                TotalQuestions   = qaList.Count
            };
            examAttemptRepository.Create(attempt);

            foreach (var qa in qaList)
            {
                questionAttemptRepository.Create(new QuestionAttempt
                {
                    Id               = Guid.NewGuid(),
                    ExamAttemptId        = attempt.Id,
                    QuestionId       = qa.QuestionId,
                    SelectedAnswerId = qa.SelectedAnswerId,
                    IsCorrect        = qa.IsCorrect,
                    Topic            = qa.Topic,
                    AnsweredAt       = qa.AnsweredAt.Kind == DateTimeKind.Utc
                                           ? qa.AnsweredAt
                                           : qa.AnsweredAt.ToUniversalTime()
                });
            }

            return RedirectToAction("Result", "Exam", new { attemptId = attempt.Id });
        }

        public class FinishDto
        {
            public bool   IsPassed             { get; set; }
            public int    TimeSpentSeconds      { get; set; }
            public string QuestionAttemptsJson  { get; set; } = "";
        }

        public class QaDto
        {
            public Guid      QuestionId       { get; set; }
            public Guid?     SelectedAnswerId { get; set; }
            public bool      IsCorrect        { get; set; }
            public string    Topic            { get; set; } = "";
            public DateTime  AnsweredAt       { get; set; }
        }

        // ── Маппер ──────────────────────────────────────────────────────────
        private static GeneratedQuestion Map(Question q, Topic t, int group) =>
            new()
            {
                Id        = q.Id,
                Text      = q.Text,
                ImageUrl  = q.ImageUrl,
                Hint      = q.Exploration ?? "",
                TopicName = t.Name,
                Group     = group,
                Answers   = q.Answers.Select(a => new GeneratedAnswer
                {
                    Id      = a.Id,
                    Text    = a.Text,
                    IsRight = a.IsRight
                }).ToList()
            };
    }

    // ── ViewModels ──────────────────────────────────────────────────────────
    public class GeneratedExamVm
    {
        public List<GeneratedQuestion> Questions      { get; set; } = new();
        public int                     InitialSeconds { get; set; }
    }

    public class GeneratedQuestion
    {
        public Guid                  Id        { get; set; }
        public string                Text      { get; set; } = "";
        public string?               ImageUrl  { get; set; }
        public string                Hint      { get; set; } = "";
        public string                TopicName { get; set; } = "";
        public int                   Group     { get; set; }
        public List<GeneratedAnswer> Answers   { get; set; } = new();
    }

    public class GeneratedAnswer
    {
        public Guid   Id      { get; set; }
        public string Text    { get; set; } = "";
        public bool   IsRight { get; set; }
    }
}
