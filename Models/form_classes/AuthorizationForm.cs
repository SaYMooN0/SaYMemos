using System.Text.RegularExpressions;

namespace SaYMemos.Models.form_classes
{
    public record class AuthorizationForm
    {
        public string Email { get; init; } = "";
        public string Password { get; init; } = "";
        public string ErrorLine { get; init; } = "";

        public bool IsFormFilled()=>
            !(string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password));
        public AuthorizationForm WithError(string error) =>
            new AuthorizationForm(this with { ErrorLine = error });

    }
}
