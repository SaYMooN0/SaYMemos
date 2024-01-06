using Microsoft.AspNetCore.Mvc;
using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.form_classes;
using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;

namespace SaYMemos.Controllers
{
    public class RegistrationController : Controller
    {
        IDatabase _db { get; init; }
        ILogger _logger { get; init; }
        IEncryptor _encryptor { get; init; }
        IEmailService _emailService { get; init; }
        public RegistrationController(IDatabase db, ILogger logger, IEncryptor encryptor, IEmailService emailService)
        {
            _db = db;
            _logger = logger;
            _encryptor = encryptor;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            if (this.GetUserIdFromIdentity() != -1)
                return RedirectToAction("index", "account");
            return View(new RegistrationForm());
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(RegistrationForm form)
        {
            var validatedForm = form?.Validate() ?? new RegistrationForm { ErrorLine = "Fill form fields" };

            if (string.IsNullOrWhiteSpace(validatedForm.Email))
                validatedForm = validatedForm with { ErrorLine = "Fill email field" };
            else if (_db.IsEmailTaken(validatedForm.Email))
                validatedForm = validatedForm with { ErrorLine = "An account with this email address already exists" };

            if (!string.IsNullOrEmpty(validatedForm.ErrorLine))
                return PartialView(viewName: "Index", validatedForm);

            return await EmailConfirmation(validatedForm);
        }


        private async Task<IActionResult> EmailConfirmation(RegistrationForm data)
        {
            string code = GenerateConfirmationCode();
            var user = UserToConfirm.FromRegistrationForm(data, _encryptor.EncryptPassword, code);
            long confirmationId = -1;
            try
            {
                confirmationId=await _db.AddUserToConfirmAsync(user);
                await this.SetConfirmationIdIdentity(confirmationId);

                if (!_emailService.TrySendConfirmationCode( data.Email,code))
                    throw new Exception("Couldn't send email with confirmation code");
            }
            catch (Exception ex)
            {
                _logger.Error("EmailConfirmation method error", ex);
                return PartialView(viewName: "Index", data with { ErrorLine = "Error during registration. Please try again later" });
            }
            
            return PartialView(viewName: "EmailConfirmation");

        }
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string? code)
        {
            const string 
                EmailConfirmationView = "EmailConfirmation",
                RegistrationError = "An error occurred during the registration process. Try to register again";

            if (string.IsNullOrWhiteSpace(code))
                return PartialView(EmailConfirmationView, "Fill code input");

            long userId = this.GetConfirmationIdFromIdentity();
            if (userId == -1)
                return PartialView(EmailConfirmationView, RegistrationError);

            if (!await _db.IsUserToConfirmExistsAsync(userId, code))
                return PartialView(EmailConfirmationView, "Wrong code");

            var userToConfirm = await _db.GetUserToConfirmById(userId);
            if (userToConfirm is null)
                return PartialView(EmailConfirmationView, RegistrationError);

            try
            {
                long newId = await _db.AddNewConfirmedUser(userToConfirm);
                await this.RemoveConfirmationIdIdentity();
                await this.SetUserIdIdentity(newId);
                await _db.DeleteUserFromConfirmAsync(userId);
                return RedirectToAction("Index", "Account");
            }
            catch (Exception ex)
            {
                _logger.Error("ConfirmEmail method error", ex);
                return PartialView(EmailConfirmationView, RegistrationError);
            }
        }

        public static string GenerateConfirmationCode()
        {
            int length = 10;
            var random = new Random();
            return new string(
                Enumerable.Range(0, length)
                .Select(_ => (char)('0' + random.Next(0, 10)))
                .ToArray());
        }


    }
}
