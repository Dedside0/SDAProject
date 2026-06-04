using Microsoft.AspNetCore.Mvc;
using SDA.Web.Models;
using System.Diagnostics;

namespace SDA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Exam()
        {
            return View();
        }

        public IActionResult Train()
        {
            return View();
        }

        public IActionResult Mistakes()
        {
            return View();
        }

        public IActionResult Stats()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
