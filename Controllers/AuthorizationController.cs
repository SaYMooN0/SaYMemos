using Microsoft.AspNetCore.Mvc;
using SaYMemos.Models.form_classes;
using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;

namespace SaYMemos.Controllers
{
    public class AuthorizationController : Controller
    {
        IDatabase _db { get; init; }
        ILogger _logger { get; init; }
        IEncryptor _encryptor { get; init; }
        public AuthorizationController(IDatabase db, ILogger logger, IEncryptor encryptor)
        {
            _db = db;
            _logger = logger;
            _encryptor = encryptor;
        }
        public IActionResult Index()
        {
            if (this.GetUserIdFromIdentity() != -1)
                return RedirectToAction("index", "account");
            return View(new AuthorizationForm());
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
            string password = _encryptor.DecryptPassword(passwordHash);
            if (form.Password != password)
                return PartialView(viewName: "Index", form.WithError("Invalid password"));

            long? userId = await _db.GetUserIdByEmail(form.Email);
            if (userId is null) 
                return PartialView(viewName: "Index", form.WithError("Invalid password"));

            this.SetUserIdIdentity((long)userId);
            await _db.UpdateLastLoginDateForUser((long)userId);
            return RedirectToAction("Index", controllerName: "Account");
        }
    }
}
