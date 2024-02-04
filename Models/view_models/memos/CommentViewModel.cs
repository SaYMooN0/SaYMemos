using SaYMemos.Models.data.entities.comments;
using SaYMemos.Models.data.entities.users;
namespace SaYMemos.Models.view_models.memos
{
    public record class CommentViewModel(
        Guid id,
        bool isParent,
        string text,
        int totalRating,
        string leavingDate,
        CommentViewModel[] childComments,
        long authorId,
        string authorNickname,
        string authorProfilePicture,
        bool isViewerAuthorized,
        bool isRated,
        bool? isUp
        )
    {
        public static CommentViewModel FromCommentForUnauthorized(Comment comment) =>
            new(
                comment.Id,
                comment.ParentCommentId is null,
                comment.Text,
                comment.CountRating(),
                comment.LeavingDate.ToString("D"),
                comment.ChildComments.Select(FromCommentForUnauthorized).ToArray(),
                comment.AuthorId,
                comment.Author.Nickname,
                comment.Author.ProfilePicturePath,
                false, false, null);
        public static CommentViewModel FromCommentForUser(Comment comment, User user)
        {
            if (user is null)
                return FromCommentForUnauthorized(comment);

            bool? isUp = user.CommentRatings
                .Where(r => r.CommentId == comment.Id)
                .Select(r => (bool?)r.IsUp)
                .FirstOrDefault();

            return new(
                comment.Id,
                comment.ParentComment is null,
                comment.Text,
                comment.CountRating(),
                comment.LeavingDate.ToString("D"),
                comment.ChildComments.Select(c => FromCommentForUser(c, user)).ToArray(),
                comment.AuthorId,
                comment.Author.Nickname,
                comment.Author.ProfilePicturePath,
                true, isUp is not null, isUp);
        }
    }
}
