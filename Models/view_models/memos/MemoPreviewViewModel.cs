using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SaYMemos.Models.data.entities.memos;
using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.view_models.memos
{
    public record MemoPreviewViewModel(
        Guid id,
        long authorId,
        string authorProfilePicture,
        string authorNickName,
        string authorComment,
        string? imagePath,
        string creationDate,
        bool isLiked,
        int likesCount,
        int commentsCount,
        bool areCommentsAvaliable,
        bool isViewerAuthorized,
        bool anyTags
        )
    {
        public static MemoPreviewViewModel FromMemo(Memo memo, User? viewer = null)
        {
            Guid[] likedMemoIds = Array.Empty<Guid>();
            bool viewerAuthorized = viewer is not null;
            if (viewerAuthorized)
                likedMemoIds = viewer.Likes.Select(l => l.MemoId).ToArray();
            return new(
                    memo.id,
                    memo.Author.Id,
                    memo.Author.ProfilePicturePath,
                    memo.Author.Nickname,
                    memo.authorComment,
                    memo.imagePath,
                    memo.creationTime.ToString("f"),
                    likedMemoIds.Contains(memo.id),
                    memo.Likes.Count,
                    memo.Comments.Count,
                    memo.areCommentsAvailable,
                    viewerAuthorized,
                    memo.Tags.Count > 0);
        }
    }
}
