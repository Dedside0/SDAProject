using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDA.Db;
using SDA.Db.Models;
using SDA.Web.Models;

namespace SDA.Web.Controllers
{
    public class AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager) : Controller
    {
        public IActionResult Authorization()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Authorization(Authorization authorization)
        {
            if (!ModelState.IsValid)
            {
                return View(authorization);
            }

            var result = await _signInManager.PasswordSignInAsync(
                authorization.Login,
                authorization.Password,
                authorization.IsRememberMe,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(authorization);
            }

            return RedirectToAction(nameof(Index), "Home");
        }


        public IActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Registration(Registration registration)
        {
            if (registration.Login == registration.Password)
            {
                ModelState.AddModelError("", "Имя и пароль не должны совпадать");
            }

            if (_userManager.FindByNameAsync(registration.Login).Result != null)
            {
                ModelState.AddModelError("", "Пользователь с таким логином уже зарегистрирован!\r\n" +
                    "Необходимо зарегистрироваться под другим логином!");
            }



            if (!ModelState.IsValid)
            {
                return View(registration);
            }

            var user = new User()
            {
                
                Email = registration.Login,
                UserName = registration.Login,
                FullName = registration.Name,
                RegistrationDateTime = DateTime.UtcNow
            };

            var result = _userManager.CreateAsync(user, registration.Password).Result;

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(registration);
            }

            var addRoleResult = _userManager.AddToRoleAsync(user, Constants.UserRoleName).Result;

            if (!addRoleResult.Succeeded)
            {
                return View(registration);
            }

            _signInManager.SignInAsync(user, true).Wait(); 

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
