using SaYMemos.Models.data.entities.memos;
using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.view_models.memos
{
    public record class MemoFullInfoViewModel(
        bool isViewerAuthorized,
        Guid id,
        long authorId,
        string authorProfilePicture,
        string authorNickName,
        string authorComment,
        string? imagePath,
        string creationDate,
        bool isLiked,
        int likesCount,
        bool areCommentsAvaliable,
        CommentViewModel[] comments,
        string[] tags
        )
    {
        public static MemoFullInfoViewModel FromMemoForUnauthorized(Memo memo) =>
            new(
                false,
                memo.id,
                memo.authorId,
                memo.Author.ProfilePicturePath,
                memo.Author.Nickname,
                memo.authorComment,
                memo.imagePath,
                memo.creationTime.ToString("D"),
                false,
                memo.Likes.Count,
                memo.areCommentsAvailable,
                memo.areCommentsAvailable ? memo.Comments
                       .Where(c => c.ParentCommentId is null)
                       .Select(CommentViewModel.FromCommentForUnauthorized).ToArray()
                : Array.Empty<CommentViewModel>(),

                memo.Tags.Take(10).Select(t => t.Value).ToArray()
                );
        public static MemoFullInfoViewModel FromMemoForUser(Memo memo, User user) =>
            new(
                true,
                memo.id,
                memo.authorId,
                memo.Author.ProfilePicturePath,
                memo.Author.Nickname,
                memo.authorComment,
                memo.imagePath,
                memo.creationTime.ToString("D"),
                user.Likes.Select(l=>l.MemoId).ToHashSet().Contains(memo.id),
                memo.Likes.Count,
                memo.areCommentsAvailable,
                memo.areCommentsAvailable ? memo.Comments
                    .Where(c => c.ParentCommentId is null)
                    .Select(c=>CommentViewModel.FromCommentForUser(c, user)).ToArray() 
                : Array.Empty<CommentViewModel>(),
                memo.Tags.Take(10).Select(t => t.Value).ToArray()
                );
    }

}
