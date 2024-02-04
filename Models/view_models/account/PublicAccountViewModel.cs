using Microsoft.AspNetCore.Components.Web;
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
            Guid[] likedMemoIds=Array.Empty<Guid>();
            bool viewerAuthorized = viewer is not null;
            if (viewerAuthorized)
                likedMemoIds = viewer.Likes.Select(l=>l.MemoId).ToArray();

            return new(user.Id,
                user.Nickname,
                user.FullName,
                user.GetLastTimeOnlineString(),
                user.ProfilePicturePath,
                user.AreLinksPrivate ? new() : user.UserLinks.ParseToNonEmptyDictionary(),
                user.PostedMemos
                .Select(memo => MemoPreviewViewModel.FromMemo(memo, likedMemoIds.Contains(memo.id), viewerAuthorized))
                .OrderByDescending(memo => memo.creationDate)
                    .ToArray(),
                  viewerAuthorized
               );
        }
            

    }
}
