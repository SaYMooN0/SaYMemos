namespace SaYMemos.Models.view_models.memos
{
    public record class CommentFormViewModel(string commentId, string error, Guid memoId)
    {
        public static CommentFormViewModel Success(Guid commentId, Guid memoId) =>
            new(commentId.ToString(), string.Empty, memoId);
        public static CommentFormViewModel Error(Guid memoId) =>
            new(string.Empty, "An error occurred while leaving a comment", memoId);
    }
}
