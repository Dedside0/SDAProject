using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SDA.Db;
using SDA.Db.Models;
using SDA.Db.Repositories;
using SDA.Web.Models;
using SDA.Web.Models.DTO;

namespace SDA.Web.Controllers
{
    [Authorize(Roles = Constants.AdminRoleName)]
    public class AdminController(ITicketRepository ticketRepository, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> EditTicket()
        {
            var temp = await ticketRepository.GetAll();
            var allTickets = temp.Select(ticket => new TicketVm()
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
                        Answers = tQuest.Question.Answers.Select(ans => new AnswerVm()
                        {
                            Id = ans.Id,
                            Text = ans.Text,
                            Order = ans.Order,
                        }).ToList()
                    }
                }).ToList()

            }).ToList();
            return View(temp);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CreateTicket([FromBody] SaveTicketDto dto)
        {
            try
            {
                var ticket = new Ticket
                {
                    Name = dto.Name,
                    Theme = dto.Theme,
                    Description = dto.Description,
                    TicketQuestions = dto.TicketQuestions.Select(tqDto =>
                    {
                        var question = new Question
                        {
                            Text = tqDto.Text,
                            ImageUrl = tqDto.ImageUrl,
                            Exploration = tqDto.Exploration,
                            Topic = tqDto.Topic,
                            Answers = tqDto.Answers.Select(aDto => new Answer
                            {
                                Text = aDto.Text,
                                IsRight = aDto.IsRight,
                                Order = aDto.Order
                            }).ToList()
                        };

                        return new TicketQuestion
                        {
                            Order = tqDto.Order,
                            Question = question
                        };
                    }).ToList()
                };
                ticketRepository.Create(ticket);
                return Ok();
            }
            catch
            {
                return BadRequest("Не удалось создать билет");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPut]
        public async Task<IActionResult> UpdateTicket([FromBody] SaveTicketDto dto)
        {
            try
            {

                var ticket = new Ticket();

                ticket.Name = dto.Name;
                ticket.Theme = dto.Theme;
                ticket.Description = dto.Description;
                ticket.Id = dto.Id;

                ticket.TicketQuestions = dto.TicketQuestions.Select(tqDto =>
                {
                    var realQuestionId = tqDto.QuestionId.ToString().StartsWith("00000000-0000-0000-0000-")
                        ? Guid.Empty
                        : tqDto.QuestionId;

                    var question = new Question
                    {
                        Id = realQuestionId,
                        Text = tqDto.Text,
                        ImageUrl = tqDto.ImageUrl,
                        Exploration = tqDto.Exploration,
                        Answers = tqDto.Answers.Select(aDto => new Answer
                        {
                            Id = aDto.Id.ToString().StartsWith("00000000-0000-0000-0000-") ? Guid.Empty : aDto.Id,
                            QuestionId = realQuestionId,
                            Text = aDto.Text,
                            IsRight = aDto.IsRight,
                            Order = aDto.Order,
                        }).ToList()
                    };

                    return new TicketQuestion
                    {
                        TicketId = dto.Id,
                        QuestionId = realQuestionId,
                        Order = tqDto.Order,
                        Question = question,
                        Ticket = ticket
                    };
                }).ToList();

                await ticketRepository.Update(ticket);
                return Ok();

            }
            catch
            {
                return BadRequest();
            }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран");


            var allowed = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
            if (!allowed.Contains(file.ContentType))
                return BadRequest("Недопустимый формат файла");


            if (file.Length > 10 * 1024 * 1024)
                return BadRequest("Файл слишком большой (максимум 5 МБ)");

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var folder = Path.Combine(_env.WebRootPath, "images", "uploads", "questions");

            var fullPath = Path.Combine(folder, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
                await file.CopyToAsync(stream);

            var url = $"/images/uploads/questions/{fileName}";
            return Ok(new { url });
        }
    }
}