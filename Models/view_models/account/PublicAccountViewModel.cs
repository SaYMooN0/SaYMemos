using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.view_models.memos;

namespace SaYMemos.Models.view_models.account
{
    public record PublicAccountViewModel(
        long userId,
        string nickname,
        string fullName,
        string lastTimeOnline,
        string profilePicturePath,
        Dictionary<string, string> links,
        MemoPreviewViewModel[] memos,
        bool isViewerAuthorized
        )
    {
        public static PublicAccountViewModel FromUser(User user,User? viewer)
        {
            return new(user.Id,
                user.Nickname,
                user.FullName,
                user.GetLastTimeOnlineString(),
                user.ProfilePicturePath,
                user.AreLinksPrivate ? new() : user.UserLinks.ParseToNonEmptyDictionary(),
                user.PostedMemos
                .Select(memo => MemoPreviewViewModel.FromMemo(memo, viewer))
                .OrderByDescending(memo => memo.creationDate)
                    .ToArray(),
                  viewer is not null
               );
        }
            

    }
}
