namespace SaYMemos.Models.view_models.memos
{
    public record class CommentVoteButtonsViewModel(
        bool? isUp,
        string commentId,
        int rating
        );
}
