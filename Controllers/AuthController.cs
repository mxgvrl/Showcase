using Microsoft.AspNetCore.Mvc;

namespace AppleShowcase.Controllers
{
    public class AuthController : Controller
    {
        // GET
        public IActionResult Auth()
        {
            // ReSharper disable once Mvc.ViewNotResolved
            return View();
        }
    }
}