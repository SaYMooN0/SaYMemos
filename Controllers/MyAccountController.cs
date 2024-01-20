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
        IImageStorageService _imgStorage { get; init; }
        public MyAccountController(IDatabase db, ILogger logger, IEncryptor encryptor, IImageStorageService imgStorage)
        {
            _db = db;
            _logger = logger;
            _enc = encryptor;
            _imgStorage = imgStorage;
        }
        public async Task<IActionResult> Index()
        {
            long? userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return Unauthorized();
            User? user = await _db.GetUserByIdAsync((long)userId);
            if (user is null)
            {
               Response.RemoveUserIdCookies();
                return Unauthorized();
            }
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
        public async Task<IActionResult> SaveProfilePictureAsync(IFormFile picture)
        {
            if (picture is null || picture.Length == 0)
                return RenderProfilePictureInput("No image received");

            (string error,Stream? imageStream) =await _imgStorage.TryConvertFormImageToStream(picture);
            if (!string.IsNullOrEmpty(error))
                return RenderProfilePictureInput(error);


            long? userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1 || userId == null)
                return Unauthorized();

            User? user = await _db.GetUserByIdAsync((long)userId);
            if (user is null)
                return Unauthorized();

            await _db.UpdateLastLoginDateForUser((long)userId);

            string savedImagePath= _imgStorage.SaveProfilePicture(imageStream, user.Id);


            await _db.SetProfilePictureForUser(user.Id, savedImagePath);
            return await RenderSettingsProfilePictureAsync();
        }

        [HttpPost]
        public async Task<IActionResult> SaveSettingsAsync(AccountSettingsForm form)
        {
            long id = this.GetUserId(_enc.DecryptId);
            if (id == -1)
                return Unauthorized();
            await _db.UpdateLastLoginDateForUser(id);

            var data = form.Validate();
            if (data.AnyErrors())
                return PartialView(viewName: "Settings", model: data);

            User? user = await _db.GetUserByIdAsync(id);
            if (user is null)
                return Unauthorized();

            await _db.UpdateUserSettings(data, id);
            Response.Headers["HX-Redirect"] = "/MyAccount";
            return Ok();
        }

        [HttpPost]
        public IActionResult RenderProfilePictureInput(string error = "") => PartialView(viewName: "ProfilePictureInput", error);
        [HttpPost]
        public async Task<IActionResult> RenderSettingsProfilePictureAsync()
        {
            long? userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return Unauthorized();
            string? picturePath = await _db.GetProfilePictureById((long)userId);
            return PartialView(viewName: "SettingsProfilePicture", picturePath);
        }
    }
}
