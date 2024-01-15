using Microsoft.AspNetCore.Mvc;

namespace SaYMemos.Controllers.Helpers
{
    public static class ControllerExtensions
    {
        public static long GetUserId(this Controller controller, Func<string, string> idDecryptionFunc)
        {
            string encryptedIdString = controller.HttpContext.Request.GetUserIdCookies();
            return !string.IsNullOrEmpty(encryptedIdString) &&
                   long.TryParse(idDecryptionFunc(encryptedIdString), out long userId) ? userId : -1;
        }
        public static void SetUserId(this Controller controller, long userId, Func<string, string> idEncryptionFunc) =>
            controller.HttpContext.Response.SetUserIdCookies(idEncryptionFunc(userId.ToString()));


        public static long GetConfirmationId(this Controller controller, Func<string, string> confirmationIdDecryptionFunc)
        {
            string encryptedConfirmationIdString = controller.HttpContext.Request.GetConfirmationIdCookies();
            return !string.IsNullOrEmpty(encryptedConfirmationIdString) &&
                   long.TryParse(confirmationIdDecryptionFunc(encryptedConfirmationIdString), out long confirmationId) ? confirmationId : -1;
        }
        public static void SetConfirmationId(this Controller controller, long confirmationId, Func<string, string> confirmationIdEncryptionFunc) =>
            controller.HttpContext.Response.SetConfirmationIdCookies(confirmationIdEncryptionFunc(confirmationId.ToString()));


        public static IActionResult UnknownUser(this Controller controller)
        {
            controller.Response.Headers["HX-Redirect"] = "/account/unknownAccount";
            return controller.RedirectToAction(actionName: "UnknownAccount", controllerName: "Account");
        }

    }
}
