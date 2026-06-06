using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SDA.Db.Models;
using SDA.Db.Repositories;
using SDA.Web.Models;
using System.Diagnostics;

namespace SDA.Web.Controllers
{
    public class HomeController(UserManager<User> userManager,
        IExamAttemptRepository examAttemptRepository,
        ILogger<HomeController> logger) : Controller
    {


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

        [Authorize]
        public IActionResult Stats()
        {
            try
            {
            var userId = Guid.Parse(userManager.GetUserId(User));
            var attempts = examAttemptRepository.GetAllByUserId(userId);
            return View(attempts);
            }
            catch
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
