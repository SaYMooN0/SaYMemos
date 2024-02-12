using Microsoft.AspNetCore.Mvc;
using SaYMemos.Models.form_classes;
using System.Text.Json;

namespace SaYMemos.Controllers.Helpers
{
    public static class ControllerExtensions
    {

        public static void SetRenderedMemoCount(this Controller controller, int count)
        {
            controller.HttpContext.Response.RemoveCookie("MemosRendered");
            controller.HttpContext.Response.SetCookie("MemoFilter", count.ToString(), 10000);
        }
        public static int GetRenderedMemoCount(this Controller controller)
        {
            string memoCount=controller.Request.GetCookie("MemosRendered");
            return string.IsNullOrEmpty(memoCount) ? 0 : Int32.TryParse(memoCount, out int res) ? res : 0;
        }
        public static void SetMemoFilter(this Controller controller, MemoFilterForm form)
        {
            controller.HttpContext.Response.RemoveCookie("MemoFilter");
            controller.HttpContext.Response.SetCookie("MemoFilter", JsonSerializer.Serialize(form), 10000);
        }

        public static MemoFilterForm GetMemoFilter(this Controller controller)
        {
            var json = controller.Request.GetCookie("MemoFilter");
            return string.IsNullOrEmpty(json) ? MemoFilterForm.Default() : JsonSerializer.Deserialize<MemoFilterForm>(json) ?? MemoFilterForm.Default();
        }

        public static void SetMemoSortOptions(this Controller controller, MemoSortOptionsForm form)
        {
            controller.HttpContext.Response.RemoveCookie("MemoSortOptions");
            controller.Response.SetCookie("MemoSortOptions", JsonSerializer.Serialize(form), 10000);
        }


        public static MemoSortOptionsForm GetMemoSortOptions(this Controller controller)
        {
            var json = controller.Request.GetCookie("MemoSortOptions");
            return string.IsNullOrEmpty(json) ? MemoSortOptionsForm.Default() : JsonSerializer.Deserialize<MemoSortOptionsForm>(json) ?? MemoSortOptionsForm.Default();
        }

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
            return controller.Ok();
        }
        public static IActionResult HxUnauthorized(this Controller controller)
        {
            controller.Response.Headers["HX-Redirect"] = "/authorization";
            return controller.Ok();
        }

    }
}
