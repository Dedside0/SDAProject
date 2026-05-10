using Microsoft.AspNetCore.Mvc;
using SDA.Db.Repositories;
using SDA.Web;

namespace SDAProject.Controllers
{
    public class ExamController(ITicketRepository ticketRepository, IQuestionRepository questionRepository) : Controller
    {
        
        public IActionResult Index()
        {
            var ticket = TestDataGenerator.GenerateTestTicket();
            return View(ticket);
        }

        

    }
}
