using Microsoft.AspNetCore.Mvc;
using SaYMemos.Controllers.Helpers;
using SaYMemos.Models.form_classes;
using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;

namespace SaYMemos.Controllers
{
    public class AuthorizationController : Controller
    {
        IDatabase _db { get; init; }
        ILogger _logger { get; init; }
        IEncryptor _enc { get; init; }
        public AuthorizationController(IDatabase db, ILogger logger, IEncryptor encryptor)
        {
            _db = db;
            _logger = logger;
            _enc = encryptor;
        }
        public IActionResult Index()
        {
            if (this.GetUserId(_enc.DecryptId) == -1)
                return View(new AuthorizationForm());
            return RedirectToAction("index", "account");
         
        }
        [HttpPost]
        public async Task<IActionResult> LogInAsync(AuthorizationForm form)
        {
            if (!form.IsFormFilled())
                return PartialView(viewName: "Index", form.WithError("Fill all fields"));
            if (!_db.IsEmailTaken(form.Email))
                return PartialView(viewName: "Index", form.WithError("Unregistered email"));
            string? passwordHash = await _db.GetPasswordHashForEmail(form.Email);
            if (string.IsNullOrWhiteSpace(passwordHash))
                return PartialView(viewName: "Index", form.WithError("Unknown user"));
            string password = _enc.DecryptPassword(passwordHash);
            if (form.Password != password)
                return PartialView(viewName: "Index", form.WithError("Invalid password"));

            long? userId = await _db.GetUserIdByEmail(form.Email);
            if (userId is null)
                return PartialView(viewName: "Index", form.WithError("Invalid password"));
            this.SetUserId( (long)userId, _enc.EncryptId);
            await _db.UpdateLastLoginDateForUser((long)userId);
            _logger.Info($"User with id {userId} has logged in");
            return RedirectToAction("Index", controllerName: "Account");
        }
    }
}
