using Microsoft.AspNetCore.Mvc;
using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;
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
        public IActionResult Index()
        {
            if(this.GetUserId(_enc.DecryptId) == -1)
                return Unauthorized();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
          throw new NotImplementedException();  
        }
    }
}
