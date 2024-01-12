using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.form_classes
{
    public record class AccountSettingsForm(
        //--main--
        string nickname,
        string profilePicturePath,
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
            string mainError=string.Empty, privacyError = string.Empty, linksError = string.Empty, additionalError = string.Empty;
            //validation
            return this with { 
                mainFieldError = mainError, 
                privacyFieldError = privacyError, 
                linksFieldError = linksError, 
                additionalInfoFieldError = additionalError };
        }
        public static  AccountSettingsForm FromUser(User user) => new(
            user.Nickname,
            user.ProfilePicturePath,
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
