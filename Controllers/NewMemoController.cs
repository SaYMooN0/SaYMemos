using Microsoft.AspNetCore.Mvc;
using SaYMemos.Controllers.Helpers;
using SaYMemos.Models.form_classes;
using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;

namespace SaYMemos.Controllers
{
    public class NewMemoController : Controller
    {
        IDatabase _db { get; init; }
        ILogger _logger { get; init; }
        IEncryptor _enc { get; init; }
        public NewMemoController(IDatabase db, ILogger logger, IEncryptor enc)
        {
            _db = db;
            _logger = logger;
            _enc = enc;
        }
        public IActionResult Index()
        {
            if (this.GetUserId(_enc.DecryptId) == -1)
                return Unauthorized();
            return View();
        }
        [HttpPost]
        public IActionResult UploadNew(MemoCreationForm data)
        {
            return Ok();
        }
    }
}
