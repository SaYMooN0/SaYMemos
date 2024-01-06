using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace SaYMemos.Controllers
{
    public static class ControllerExtensions
    {
        public static long GetUserIdFromIdentity(this Controller controller) =>
            long.TryParse(controller.HttpContext.User.FindFirst("userId")?.Value, out long userId) ? userId : -1;

        public static Task SetUserIdIdentity(this Controller controller, long userId) =>
            SetIdentity(controller, "userId", userId.ToString());

        public static long GetConfirmationIdFromIdentity(this Controller controller) =>
            long.TryParse(controller.HttpContext.User.FindFirst("confirmationId")?.Value, out long confirmationId) ? confirmationId : -1;

        public static Task SetConfirmationIdIdentity(this Controller controller, long confirmationId) =>
            SetIdentity(controller, "confirmationId", confirmationId.ToString());

        public static async Task RemoveConfirmationIdIdentity(this Controller controller)
        {
            ClaimsIdentity identity = new(controller.HttpContext.User.Claims.Where(c => c.Type != "confirmationId"), "ApplicationCookie");
            ClaimsPrincipal principal = new(identity);
            await controller.HttpContext.SignInAsync(principal);
        }

        private static async Task SetIdentity(Controller controller, string claimType, string claimValue)
        {
            var claims = new List<Claim>(controller.HttpContext.User.Claims);
            claims.RemoveAll(c => c.Type == claimType);
            claims.Add(new Claim(claimType, claimValue));
            ClaimsPrincipal principal = new(new ClaimsIdentity(claims, "ApplicationCookie"));
            await controller.HttpContext.SignInAsync(principal);
        }
    }
}
