using Microsoft.AspNetCore.Mvc;

namespace SaYMemos.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            if(this.GetUserIdFromIdentity() == -1)
                return RedirectToAction("index", "registration");
            return View();
        }
        public IActionResult LogOut()
        {
            throw new NotImplementedException();
        }
    }
}
