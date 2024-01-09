using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.view_models.account
{
    public record PublicAccountViewModel(
        string nickname,
        string lastTimeOnline,
        string profilePicturePath,
        UserLinks links,
        string FullName,
        string registrationDate,
        string additionalInfo,
        string Hobbies
        )
    {
        public static PublicAccountViewModel FromUser(User user) =>
            new(user.Nickname, 
                user.GetLastTimeOnlineString(), 
                user.ProfilePicturePath,
                user.GetUserLinksToShow(),
                user.AdditionalInfo.FullName,
                user.AdditionalInfo.RegistrationDate.ToString("D"),
                user.AdditionalInfo.AdditionalInfo,
                user.AdditionalInfo.Hobbies);
    }
}
