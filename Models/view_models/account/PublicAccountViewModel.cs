using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.view_models.account
{
    public record PublicAccountViewModel(
        string nickname,
        string lastTimeOnline,
        string profilePicturePath,
        Dictionary<string, string> links,
        string fullName,
        string registrationDate,
        string additionalInfo,
        string hobbies
        )
    {
        public static PublicAccountViewModel FromUser(User user) =>
            new(user.Nickname,
                user.GetLastTimeOnlineString(),
                user.ProfilePicturePath,
                user.AreLinksPrivate ? new() : user.UserLinks.ParseToNonEmptyDictionary(),
                user.AdditionalInfo.FullName,
                user.AdditionalInfo.RegistrationDate.ToString("D"),
                user.AdditionalInfo.AdditionalInfo,
                user.AdditionalInfo.Hobbies);
    }
}
