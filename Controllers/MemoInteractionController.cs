using Microsoft.AspNetCore.Mvc;

namespace SaYMemos.Controllers
{
    public class MemoInteractionController : Controller
    {
        [HttpPost]
        public IActionResult LeaveComment(string memoId, string comment)
        {
            return View();
        }
    }
}
