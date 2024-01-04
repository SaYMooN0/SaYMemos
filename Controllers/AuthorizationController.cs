using Microsoft.AspNetCore.Mvc;

namespace SaYMemos.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
