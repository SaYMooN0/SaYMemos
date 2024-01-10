using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.view_models.account
{
    public record PrivateAccountViewModel(string nickname, string lastTimeOnline, string profilePicturePath, Dictionary<string,string> links)
    {
        public static PrivateAccountViewModel FromUser(User user) =>
            new(user.Nickname, user.GetLastTimeOnlineString(), user.ProfilePicturePath, user.GetUserLinksDictionary());
    }
}
