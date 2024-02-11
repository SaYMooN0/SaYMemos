namespace SaYMemos.Controllers.Helpers
{
    public static class CookieManagement
    {
        public static void SetUserIdCookies(this HttpResponse response, string userId) =>
            response.SetCookie("UserId", userId);

        public static string GetUserIdCookies(this HttpRequest request) =>
            request.GetCookie("UserId");

        public static void RemoveUserIdCookies(this HttpResponse response) =>
            response.RemoveCookie("UserId");


        public static void SetConfirmationIdCookies(this HttpResponse response, string confirmationId) =>
            response.SetCookie("ConfirmationId", confirmationId);

        public static string GetConfirmationIdCookies(this HttpRequest request) =>
            request.GetCookie("ConfirmationId");

        public static void RemoveConfirmationIdCookies(this HttpResponse response) =>
            response.RemoveCookie("ConfirmationId");


        public static void SetCookie(this HttpResponse response, string key, string value, int? expireTime = null)
        {
            var option = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true
            };

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddYears(20);

            response.Cookies.Append(key, value, option);
        }

        public static string GetCookie(this HttpRequest request, string key) =>
            request.Cookies.TryGetValue(key, out string value) ? value : string.Empty;

        public static void RemoveCookie(this HttpResponse response, string key) =>
            response.Cookies.Delete(key);


    }
}
