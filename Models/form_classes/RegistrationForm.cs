using System.Text.RegularExpressions;

namespace SaYMemos.Models.form_classes
{
    public record class RegistrationForm
    {
        public string Email { get; init; } = "";
        public string Password { get; init; } = "";
        public string RepeatedPassword { get; init; } = "";
        public string Nickname { get; init; } = "";
        public string ErrorLine { get; init; } = "";

        public RegistrationForm Validate()
        {
            return
                string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RepeatedPassword) || string.IsNullOrEmpty(Nickname) ? this with { ErrorLine = "Fill all fields" } :

                !Regex.IsMatch(Email, @"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$") ? this with { ErrorLine = "Invalid email format" } :

                Password.Length < 8 || Password.Length > 30 ? this with { ErrorLine = "Password must be between 8 and 30 characters long" } :

                Password != RepeatedPassword ? this with { ErrorLine = "Passwords do not match" } :

                Nickname.Length < 3 || Nickname.Length > 30 ? this with { ErrorLine = "Nickname must be between 3 and 30 characters long" } :

                !Regex.IsMatch(Nickname, @"^[a-zA-Z0-9 _<>\[\]{}]+$") ? this with { ErrorLine = "Nickname contains invalid characters" } :

                this;
        }
    }
}


