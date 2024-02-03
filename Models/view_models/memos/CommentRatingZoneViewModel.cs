namespace SaYMemos.Models.view_models.memos
{
    public record class CommentRatingZoneViewModel(
        bool isRated,
        bool? isUp,
        string commentId,
        int rating
        );
}
