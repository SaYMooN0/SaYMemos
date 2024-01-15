using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.view_models.account
{
    public record PublicAccountViewModel(
        long userId,
        string nickname,
        string lastTimeOnline,
        string profilePicturePath,
        Dictionary<string, string> links,
        string fullName
        )
    {
        public static PublicAccountViewModel FromUser(User user) =>
            new(user.Id,
                user.Nickname,
                user.GetLastTimeOnlineString(),
                user.ProfilePicturePath,
                user.AreLinksPrivate ? new() : user.UserLinks.ParseToNonEmptyDictionary(),
                user.FullName);

    }
}
