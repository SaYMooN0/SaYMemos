using SaYMemos.Models.data.entities.memos;

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
        bool isViewerAuthorized
        )
    {
        public static MemoPreviewViewModel FromMemo(Memo memo, bool isLiked, bool isAuthorized) => new(
            memo.id,
            memo.Author.Id,
            memo.Author.ProfilePicturePath,
            memo.Author.Nickname,
            memo.authorComment,
            memo.imagePath,
            memo.creationTime.ToString("f"),
            isLiked,
            memo.Likes.Count,
            memo.Comments.Count,
            memo.areCommentsAvailable,
            isAuthorized);
    }
}
