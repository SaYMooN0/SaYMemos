using SaYMemos.Models.data.entities.comments;
namespace SaYMemos.Models.view_models.memos
{
    public record class CommentViewModel(
        bool isParent,
        string text,
        int totalRating,
        CommentViewModel[] childComments,
        long authorId,
        string authorNickname,
        string authorProfilePicture
        )
    {
        public static CommentViewModel FromComment(Comment comment) =>
            new(
                comment.parentCommentId is null,
                comment.text,
                comment.CountRating(),
                comment.ChildComments.Select(FromComment).ToArray(),
                comment.authorId,
                comment.Author.Nickname,
                comment.Author.ProfilePicturePath);
    }
}
