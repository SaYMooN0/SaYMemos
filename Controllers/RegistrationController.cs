using Microsoft.AspNetCore.Mvc;
using SaYMemos.Models.form_classes;
using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;

namespace SaYMemos.Controllers
{
    public class RegistrationController : Controller
    {
        IDatabase _db { get; init; }
        ILogger logger { get; init; }
        public RegistrationController(IDatabase db, ILogger logger)
        {
            _db = db;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            return View(new RegistrationForm());
        }
        [HttpPost]
        public IActionResult SignUp(RegistrationForm form)
        {
            var validatedForm = form?.Validate() ?? new RegistrationForm { ErrorLine = "Fill form fields" };

            if (string.IsNullOrWhiteSpace(validatedForm.Email))
                validatedForm = validatedForm with { ErrorLine = "Fill email field" };
            else if (_db.IsEmailTaken(validatedForm.Email))
                validatedForm = validatedForm with { ErrorLine = "An account with this email address already exists" };

            if (!string.IsNullOrEmpty(validatedForm.ErrorLine))
                return PartialView(viewName: "Index", validatedForm);

            return EmailConfirmation(validatedForm);
        }


        private IActionResult EmailConfirmation(RegistrationForm data)
        {
            //send confirmation code
            //add to confirmation table
            //bla-bla
            return PartialView(viewName:"EmailConfirmation");

        }
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string? Code)
        {
            if (string.IsNullOrWhiteSpace(Code)) 
                return PartialView("EmailConfirmation", "Fill code input");
            //register user
            throw new NotImplementedException();
        }

    }
}
