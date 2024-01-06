using Microsoft.AspNetCore.Mvc;

namespace SaYMemos.Controllers
{
    public class NewMemoController : Controller
    {
        public IActionResult Index()
        {
            if (this.GetUserIdFromIdentity() == -1)
                return Unauthorized();
            return View();
        }
    }
}
