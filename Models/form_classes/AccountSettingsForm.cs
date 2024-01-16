using SaYMemos.Models.data.entities.users;
using System.Text.RegularExpressions;

namespace SaYMemos.Models.form_classes
{
    public record class AccountSettingsForm(
     string nickname,
     string fullName,

     string githubLink,
     string telegramLink,
     string youtubeLink,
     string discordLink,

     string aboutMe,
     string hobbies,

     bool isAccountPrivate,
     bool areLinksPrivate,
     bool isLastLoginDatePrivate,
     string mainFieldError,
     string linksFieldError,
     string additionalInfoFieldError)
    {

        public bool AnyErrors() => !(
                string.IsNullOrEmpty(mainFieldError) &&
                string.IsNullOrEmpty(linksFieldError) &&
                string.IsNullOrEmpty(additionalInfoFieldError));


        public AccountSettingsForm Validate()
        {
            var mainError = ValidateMainFields();
            var linksError = ValidateLinks(githubLink, telegramLink, youtubeLink, discordLink);
            var additionalError = ValidateAdditionalInfo();

            return this with
            {
                mainFieldError = mainError,
                linksFieldError = linksError,
                additionalInfoFieldError = additionalError
            };
        }

        private string ValidateMainFields()
        {
            if (nickname.Length < 3 || nickname.Length > 30)
                return "Nickname must be between 3 and 30 characters long";
            else if (!Regex.IsMatch(nickname, @"^[a-zA-Z0-9 _<>\[\]{}]+$"))
                return "Nickname contains invalid characters";
            else if (!string.IsNullOrEmpty(fullName) && fullName.Length > 127)
                return $"Sorry, but fullname can't be more than 127 characters. Current length: {fullName.Length}";

            return string.Empty;
        }

        private string ValidateLinks(params string[] links)
        {
            foreach (var link in links)
            {
                if (!string.IsNullOrEmpty(link) && link.Length > 127)
                    return "Links can't be longer than 127 characters. Please use link shortening services";
            }
            return string.Empty;
        }

        private string ValidateAdditionalInfo()
        {
            if (!string.IsNullOrEmpty(hobbies) && hobbies.Length > 255)
                return $"'Hobbies' field can't be more than 255 characters. Current length: {hobbies.Length}";
            else if (!string.IsNullOrEmpty(aboutMe) && aboutMe.Length > 255)
                return $"'About Me' field can't be more than 255 characters. Current length: {aboutMe.Length}";

            return string.Empty;
        }

        public static AccountSettingsForm FromUser(User user) => new(
            user.Nickname,
            user.FullName,
            user.UserLinks.GitHub, user.UserLinks.Telegram, user.UserLinks.Youtube, user.UserLinks.Discord,
            user.AdditionalInfo.AboutMe, user.AdditionalInfo.Hobbies,
            user.IsAccountPrivate,
            user.AreLinksPrivate,
            user.IsLastLoginDatePrivate,
            string.Empty, string.Empty, string.Empty);
    }
}
