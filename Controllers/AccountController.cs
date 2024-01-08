using Microsoft.AspNetCore.Mvc;
using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;
using SaYMemos.Controllers.Helpers;
using System.Net;
using SaYMemos.Models.data.entities.users;

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
                return View(viewName: "MyAccount");
            }

            User? user = await _db.GetUserByIdAsync(userId.Value);
            if (user is null)
            {
                return View(viewName:"UnknownAccount");
            }

            return user.IsAccountPrivate ? PrivateAccountView(user) : PublicAccountView(user);
        }

        private IActionResult PrivateAccountView(User user)
        {
            return View(viewName: "PrivateAccount");
        }
        private IActionResult PublicAccountView(User user)
        {
            return View(viewName: "PublicAccount");
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
       
    }
}
