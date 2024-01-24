using SaYMemos.Models.data.entities.comments;
namespace SaYMemos.Models.view_models.memos
{
    public record class CommentViewModel(
        long authorId,
        string authorProfilePicture,
        string text,
        int rating,
        CommentViewModel[] childComments
        )
    {
        public static CommentViewModel FromComment(Comment comment) =>
            new(comment.authorId, comment.Author.ProfilePicturePath, comment.text,comment.CountRating(), comment.ChildComments.Select(FromComment).ToArray() );
    }
}
