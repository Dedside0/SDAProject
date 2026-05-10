using Microsoft.AspNetCore.Mvc;

namespace SDA.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult EditTicket()
        {
            return View();
        }
    }
}
