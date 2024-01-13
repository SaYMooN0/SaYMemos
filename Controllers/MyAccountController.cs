using Microsoft.AspNetCore.Mvc;
using SaYMemos.Controllers.Helpers;
using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.form_classes;
using SaYMemos.Models.view_models.account;
using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;

namespace SaYMemos.Controllers
{
    public class MyAccountController : Controller
    {
        IDatabase _db { get; init; }
        ILogger _logger { get; init; }
        IEncryptor _enc { get; init; }
        public MyAccountController(IDatabase db, ILogger logger, IEncryptor encryptor)
        {
            _db = db;
            _logger = logger;
            _enc = encryptor;
        }
        public async Task<IActionResult> Index()
        {
            long? userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return Unauthorized();
            User? user = await _db.GetUserByIdAsync((long)userId);
            if (user is null)
                return Unauthorized();
            return View(MyAccountViewModel.FromUser(user));
        }
        [HttpPost]
        public async Task<IActionResult> Settings()
        {
            long? userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return Unauthorized();
            User? user = await _db.GetUserByIdAsync((long)userId);
            if (user is null)
                return Unauthorized();

            return View(AccountSettingsForm.FromUser(user));
        }

        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            long id = this.GetUserId(_enc.DecryptId);
            if (id == -1)
                return Unauthorized();
            await _db.UpdateLastLoginDateForUser(id);
            HttpContext.Response.RemoveUserIdCookies();
            _logger.Info($"User with id {id} has logged out");

            Response.Headers["HX-Redirect"] = "/authorization";
            return Ok();
        }


       
        [HttpPost]
        public IActionResult SaveProfilePicture(IFormFile picture)
        {
            //  else if (AnyProfilePicture())
            //{
            //    if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(GetProfilePictureExtension()))

            //        mainError = "Unsupported file type";
            //}
            //public bool AnyProfilePicture() => newProfilePicture != null && newProfilePicture.Length > 0;
            //public string GetProfilePictureExtension() => AnyProfilePicture() ? Path.GetExtension(newProfilePicture.FileName).ToLowerInvariant() : string.Empty;
            return Ok();
        }
        [HttpPost]
        public IActionResult SaveSettings(AccountSettingsForm form)
        {
            var data = form.Validate();
            if (data.AnyErrors())
                return PartialView("Settings", data);


            //saving changes

            Response.Headers["HX-Redirect"] = "/myaccount";
            return Ok();
        }

        [HttpPost]
        public IActionResult RenderProfilePictureInput() => PartialView(viewName: "ProfilePictureInput");
        [HttpPost]
        public IActionResult RenderSettingsProfilePicture() => PartialView(viewName: "SettingsProfilePicture");

    }
}
