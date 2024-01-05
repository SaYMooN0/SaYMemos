using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SaYMemos.Controllers
{
    public static class ControllerExtensions
    {
        public static int GetUserIdFromIdentity(this Controller controller) =>
            int.TryParse(controller.HttpContext.User.FindFirst("userId")?.Value, out int userId) ? userId : -1;
        private static async Task SetUserIdIdentity(this Controller controller, int userId)
        {
            ClaimsIdentity identity = new([new Claim("userId", userId.ToString())], "ApplicationCookie");
            ClaimsPrincipal principal = new(identity);
            await controller.HttpContext.SignInAsync(principal);
        }

    }


}
