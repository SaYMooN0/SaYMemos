using Microsoft.AspNetCore.Mvc;

namespace SaYMemos.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
