using Microsoft.AspNetCore.Mvc;
using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;
using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.view_models.account;
using SaYMemos.Controllers.Helpers;

namespace SaYMemos.Controllers
{
    public class AccountController : Controller
    {
        IDatabase _db { get; init; }
        ILogger _logger { get; init; }
        IEncryptor _enc { get; init; }
        public AccountController(IDatabase db, ILogger logger, IEncryptor encryptor)
        {
            _db = db;
            _logger = logger;
            _enc = encryptor;
        }
        public async Task<IActionResult> Index(int? userId = null)
        {
            long viewerId = this.GetUserId(_enc.DecryptId);


            if (userId is null)
            {
                if (viewerId == -1)
                    return RedirectToAction(actionName: "Index", controllerName: "Authorization");
                return RedirectToAction(actionName: "Index", controllerName: "MyAccount");
            }

            User? user = await _db.GetUserById(userId.Value);
            if (user is null)
                return this.UnknownUser();

            if(userId==viewerId)
                return RedirectToAction(actionName: "Index", controllerName: "MyAccount");

            User? viewer = await _db.GetUserById(viewerId);

            return user.IsAccountPrivate ? PrivateAccountView(user) : PublicAccountView(user, viewer);
        }


        [HttpPost]
        public async Task<IActionResult> RenderAdditionalInfo(long? userId = null)
        {
            if (userId is null)
                return this.UnknownUser();
            User? user = await _db.GetUserById((long)userId);
            if (user is null || user.IsAccountPrivate)
                return this.UnknownUser();
            return PartialView(viewName: "AllUserInfo", AllUserInfoViewModel.FromUser(user));
        }
        private IActionResult PrivateAccountView(User user) =>
            View(viewName: "PrivateAccount", PrivateAccountViewModel.FromUser(user));
        private IActionResult PublicAccountView(User user, User? viewer) =>
            View(viewName: "PublicAccount", PublicAccountViewModel.FromUser(user, viewer));

        public IActionResult UnknownAccount() => View();
    }
}
