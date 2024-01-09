using Microsoft.AspNetCore.Mvc;

namespace SaYMemos.Controllers
{
    public class PeopleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
