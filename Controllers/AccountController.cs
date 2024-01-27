using Microsoft.AspNetCore.Mvc;
using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;
using SaYMemos.Controllers.Helpers;
using System.Net;
using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.view_models.account;

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
        public async Task<IActionResult> IndexAsync(int? userId = null)
        {
            if (userId is null)
            {
                if (this.GetUserId(_enc.DecryptId) == -1)
                    return Unauthorized();
                return RedirectToAction(actionName: "Index", controllerName: "MyAccount");
            }

            User? user = await _db.GetUserById(userId.Value);
            if (user is null)
                return this.UnknownUser();

            return user.IsAccountPrivate ? PrivateAccountView(user) : PublicAccountView(user);
        }


        [HttpPost]
        public async Task<IActionResult> RenderAdditionalInfoAsync(long? userId = null)
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
        private IActionResult PublicAccountView(User user) =>
            View(viewName: "PublicAccount", PublicAccountViewModel.FromUser(user));

        public IActionResult UnknownAccount() => View();
    }
}
