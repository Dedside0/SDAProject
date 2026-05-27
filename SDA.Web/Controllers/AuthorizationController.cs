using Microsoft.AspNetCore.Mvc;

namespace SDA.Web.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
