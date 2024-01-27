using Microsoft.AspNetCore.Mvc;
using SaYMemos.Controllers.Helpers;
using SaYMemos.Models.data.entities.memos;
using SaYMemos.Models.data.entities.users;
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
        IImageStorageService _imgStorage { get; init; }
        public NewMemoController(IDatabase db, ILogger logger, IEncryptor enc, IImageStorageService imgStorage)
        {
            _db = db;
            _logger = logger;
            _enc = enc;
            _imgStorage = imgStorage;
        }
        public async Task<IActionResult> Index()
        {

            long? userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1 || userId == null)
                return Unauthorized();

            User? user = await _db.GetUserById((long)userId);
            if (user is null)
                return Unauthorized();

            await _db.UpdateLastLoginDateForUser((long)userId);

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveNewMemo(MemoCreationForm form)
        {

            long? userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1 || userId == null)
                return Unauthorized();

            User? user = await _db.GetUserById((long)userId);
            if (user is null)
                return Unauthorized();

            await _db.UpdateLastLoginDateForUser((long)userId);

            form = form.Validate();
            if (form.HasError())
                return PartialView(viewName: "Index", model: form);


            string savedImagePath = string.Empty;

            if (form.HasImage())
            {
                (string imageError, Stream? imageStream) = await _imgStorage.TryConvertFormImageToStream(form.image);
                if (!string.IsNullOrEmpty(imageError))
                    return PartialView(viewName: "Index", model: (form with { error = imageError }));

                savedImagePath = _imgStorage.SaveMemoImage(imageStream, user.Id);
            }
            await _db.AddNewMemo(user.Id, form.authorComment, form.areCommentsAvailable, savedImagePath,  form.hashtags);

            Response.Headers["HX-Redirect"] = "/MyAccount";
            return Ok();
        }

        [HttpPost]
        public IActionResult RenderNewHashtagInput()
        {
            if (this.GetUserId(_enc.DecryptId) == -1)
                return Unauthorized();
            return PartialView(viewName: "HashTagInput", model: $"hashtag-{Guid.NewGuid()}");
        }
    }
}
