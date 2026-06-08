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
    public class AdminController(
        ITicketRepository ticketRepository,
        ITopicRepository themeRepository,
        IWebHostEnvironment _env) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> EditTicket()
        {
            var temp = await ticketRepository.GetAll();
            var allTickets = temp?.Select(ticket => new TicketVm()
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
                        ImageUrl = tQuest.Question.ImageUrl!,
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
                var ticket = dto.ToTicket();

                ticketRepository.Create(ticket);
                return Ok();
            }
            catch
            {
                return BadRequest("Не удалось создать билет");
            }
        }
         
        [ValidateAntiForgeryToken]
        [HttpPatch]
        public async Task<IActionResult> UpdateTicket([FromBody] SaveTicketDto dto)
        {
            try
            {
                var ticket = dto.ToTicket();

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
                return BadRequest("Файл слишком большой (максимум 10 МБ)");

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var folder = Path.Combine(_env.WebRootPath, "images", "uploads", "questions");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var fullPath = Path.Combine(folder, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
                await file.CopyToAsync(stream);

            var url = $"/images/uploads/questions/{fileName}";
            return Ok(new { url });
        }


        [HttpGet]
        public async Task<IActionResult> EditTheme()
        {
            var allThemes = (await themeRepository.GetAll()).OrderBy(x => x.Group).ToList();
            return View(allThemes);
        }


        public class TopicDto
        {
            public string Name { get; set; }
            public int Group { get; set; }
            public int Id { get; set; }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateTheme([FromBody] Topic dto)
        {
            try
            {
                await themeRepository.Create(dto);
                var res = await themeRepository.GetByName(dto.Name);
                return Json(new { id = res.Id, name = res.Name, group = res.Group });
            }
            catch
            {
                return BadRequest();
            }
        }


        [ValidateAntiForgeryToken]
        [HttpPatch]
        public async Task<IActionResult> UpdateTheme([FromBody] Topic theme)
        {
            try
            {
                await themeRepository.Update(theme);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [ValidateAntiForgeryToken]
        [HttpDelete]
        public async Task<IActionResult> DeleteTheme(int id)
        {
            try
            {
                await themeRepository.Delete(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllThemes()
        {
            try
            {
                var themes = await themeRepository.GetAll();
                var dto = themes?.Select(Mapper.ToTopicDto).ToList();
                return Json(dto);
            }
            catch
            {
                return BadRequest();
            }
        }


    }
}