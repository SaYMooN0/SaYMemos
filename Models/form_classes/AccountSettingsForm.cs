using SaYMemos.Models.data.entities.users;
using System.Text.RegularExpressions;

namespace SaYMemos.Models.form_classes
{
    public record class AccountSettingsForm(
        //--main--
        string nickname,
        string fullName,
        //--privacy--
        bool isAccountPrivate,
        bool areLinksPrivate,
        bool isLastLoginDatePrivate,
        //--links--
        string githubLink,
        string telegramLink,
        string youtubeLink,
        string discordLink,
        //--additional info--
        string additionalInfo,
        string hobbies,
        //--errors
        string mainFieldError,
        string privacyFieldError,
        string linksFieldError,
        string additionalInfoFieldError)
    {
        public bool AnyErrors() => !(
            string.IsNullOrEmpty(mainFieldError) &&
            string.IsNullOrEmpty(privacyFieldError) &&
            string.IsNullOrEmpty(linksFieldError) &&
            string.IsNullOrEmpty(additionalInfoFieldError));

       
        public AccountSettingsForm Validate()
        {
            string mainError = string.Empty, privacyError = string.Empty, linksError = string.Empty, additionalError = string.Empty;
            

            if (nickname.Length < 3 || nickname.Length > 30)
                mainError = "Nickname must be between 3 and 30 characters long";
            else if (!Regex.IsMatch(nickname, @"^[a-zA-Z0-9 _<>\[\]{}]+$"))
                mainError = "Nickname contains invalid characters";

          
            

            //check string fields length


            return this with
            {
                mainFieldError = mainError,
                privacyFieldError = privacyError,
                linksFieldError = linksError,
                additionalInfoFieldError = additionalError
            };
        }
        public static AccountSettingsForm FromUser(User user) => new(
            user.Nickname,
            user.AdditionalInfo.FullName,
            user.IsAccountPrivate,
            user.AreLinksPrivate,
            user.IsLastLoginDatePrivate,
            user.UserLinks.GithubLink,
            user.UserLinks.TelegramLink,
            user.UserLinks.YoutubeLink,
            user.UserLinks.DiscordLink,
            user.AdditionalInfo.AdditionalInfo,
            user.AdditionalInfo.Hobbies,
            string.Empty, string.Empty, string.Empty, string.Empty);

    }
}
