using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SDA.Db.Repositories;
using SDA.Web;
using SDA.Web.Models;
using SDA.Web.Models.DTO;

namespace SDAProject.Controllers
{
    [Authorize]
    public class ExamController(
        ITicketRepository ticketRepository,
        IQuestionRepository questionRepository,
        IUserRepository userRepository) : Controller
    {

        public async Task<IActionResult> Index()
        {
            //var ticket = TestDataGenerator.GenerateTestTicket();
            var ticket = (await ticketRepository.GetAll())[0];
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
                        Answers = tQuest.Question.Answers.Select(ans => new AnswerVm()
                        {
                            Id = ans.Id,
                            Text = ans.Text
                        }).ToList()
                    }
                }).ToList()

            };
            return View(ticketVm);
        }

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
        public async Task<IActionResult> Finish([FromBody] CheckExamDTO dto)
        {
            if (dto is null)
                return BadRequest();
            int correct = 0;
            var answerCount = dto.CheckAnswers.Count();
            for (int i = 0; i < answerCount; i++)
            {
                var question = questionRepository.GetById(dto.CheckAnswers[i].QuestionId);
                if (question is null)
                    return BadRequest();

                var rightAnswer = question.Answers.FirstOrDefault(x => x.IsRight);
                if (rightAnswer is null)
                    return BadRequest();

                if (rightAnswer.Id == dto.CheckAnswers[i].AnswerId)
                    correct++;
            }
            return Ok(new { correct, incorrect = answerCount - correct, percent = correct / answerCount, passed = correct>=18 });
        }


    }
}
