using Microsoft.AspNetCore.Mvc;

namespace SDAProject.Controllers
{
    public class ExamController(ICardRepo) : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        

    }
}
