using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.view_models.account
{
    public record class AllUserInfoViewModel(
        long userId,
        string nickname,
        string lastTimeOnline,
        string fullName,
        bool areLinksPrivate,
        UserLinks links,
        string registrationDate,
        string aboutMe,
        string hobbies)
    {
        public static AllUserInfoViewModel FromUser(User user) =>
            new(
                user.Id,
                user.Nickname,
                user.GetLastTimeOnlineString(),
                user.FullName,
                user.AreLinksPrivate,
                user.UserLinks,
                user.AdditionalInfo.RegistrationDate.ToString("D"),
                user.AdditionalInfo.AboutMe,
                user.AdditionalInfo.Hobbies);
    }
}