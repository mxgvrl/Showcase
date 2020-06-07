using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AppleShowcase.Data.Models;
using AspNetCore.Identity.Mongo.Mongo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AppleShowcase.Controllers
{
public class AccountController : Controller
{
        private UserService db;
        public AccountController(UserService context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            // ReSharper disable once Mvc.ViewNotResolved
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users
                    .FirstOrDefaultAsync(u => u.Name == model.Name && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация
                    return RedirectToAction("Home", "Home");
                }
                ModelState.AddModelError("", "Invalid name or(and) password");
            }
            // ReSharper disable once Mvc.ViewNotResolved
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            // ReSharper disable once Mvc.ViewNotResolved
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Name == model.Name);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new User
                    {
                        Name = model.Name,
                        Password = model.Password,
                    };
                    await db.Users.InsertOneAsync(user);
                    await Authenticate(user);
                    return RedirectToAction("Home", "Home");
                }
                else
                    ModelState.AddModelError("", "Incorrect data");
            }
            // ReSharper disable once Mvc.ViewNotResolved
            return View(model);
        }
 
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("name", user.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}