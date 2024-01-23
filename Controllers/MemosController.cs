using Microsoft.AspNetCore.Mvc;
using SaYMemos.Models.view_models;
using System.Diagnostics;

namespace SaYMemos.Controllers
{
    public class MemosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WithTag(string tag)
        {
            return View(viewName:"Index");
        }
        public IActionResult WithTags(string[] tags)
        {
            return View(viewName: "Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
