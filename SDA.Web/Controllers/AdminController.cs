using Microsoft.AspNetCore.Mvc;
using SDA.Db.Models;
using SDA.Db.Repositories;
using SDA.Web.Models;
using SDA.Web.Models.DTO;

namespace SDA.Web.Controllers
{
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
                            Text = ans.Text
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

                var id = Guid.NewGuid();
                var ticket = new Ticket
                {
                    Id = id,
                    Name = dto.Name,
                    Difficulty = dto.Difficulty,
                    Description = dto.Description,
                    TicketQuestions = dto.TicketQuestions.Select(tqDto =>
                    {
                        var realQuestionId = Guid.NewGuid();
                        var question = new Question
                        {
                            Id = realQuestionId,
                            Text = tqDto.Text,
                            ImageUrl = tqDto.ImageUrl,
                            Exploration = tqDto.Exploration,
                            Answers = tqDto.Answers.Select(aDto => new Answer
                            {
                                Id = Guid.NewGuid(),
                                QuestionId = realQuestionId,
                                Text = aDto.Text,
                                IsRight = aDto.IsRight
                            }).ToList()
                        };
                        return new TicketQuestion
                        {
                            TicketId = id,
                            QuestionId = realQuestionId,
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

                var ticket = await ticketRepository.GetById(dto.Id);
                if (ticket is null)
                    return NotFound();

                ticket.Name = dto.Name;
                ticket.Difficulty = dto.Difficulty;
                ticket.Description = dto.Description;

                ticket.TicketQuestions = dto.TicketQuestions.Select(tqDto =>
                {
                    var question = new Question
                    {
                        
                        Id = tqDto.QuestionId,
                        Text = tqDto.Text,
                        ImageUrl = tqDto.ImageUrl,
                        Exploration = tqDto.Exploration,
                        Answers = tqDto.Answers.Select(aDto => new Answer
                        {
                            Id = aDto.Id,
                            QuestionId = tqDto.QuestionId,
                            Text = aDto.Text,
                            IsRight = aDto.IsRight
                        }).ToList()
                    };

                    return new TicketQuestion
                    {
                        TicketId = dto.Id,
                        QuestionId = question.Id,
                        Order = tqDto.Order,
                        Question = question
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

            var url = $"/uploads/questions/{fileName}";
            return Ok(new { url });
        }
    }
}