namespace SaYMemos.Models.view_models.memos
{
    public record class CommentVoteButtonsViewModel(
        bool isRated,
        bool isUp,
        string commentId,
        string memoId
        );
}
