namespace SaYMemos.Models.view_models.memos
{
    public record class CommentReplyFormViewModel(
        string userToReply,
        string parentCommentId,
        string text,
        string errorLine
        )

    {
    }
}
