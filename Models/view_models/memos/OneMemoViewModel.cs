using SaYMemos.Models.data.entities.memos;

namespace SaYMemos.Models.view_models.memos
{
    public record OneMemoViewModel(
        Guid id,
        long authorId,
        string authorProfilePicture,
        string authorNickName,
        string authorComment,
        string? imagePath,
        string creationDate,
        string[] tags,


        bool isLiked,
        int likesCount,
        bool areCommentsAvaliable,
        CommentViewModel[] comments
        )
    {
        public static OneMemoViewModel FromMemo(Memo memo, bool isLiked) => new(
            memo.id,
            memo.Author.Id,
            memo.Author.ProfilePicturePath,
            memo.Author.Nickname,
            memo.authorComment,
            memo.imagePath,
            memo.creationTime.ToString("f"),
            memo.Tags.Select(i => i.Value).ToArray(),
            isLiked,
            memo.Likes.Count,
            memo.areCommentsAvailable,
            memo.Comments.Select(CommentViewModel.FromComment).ToArray() ?? Array.Empty<CommentViewModel>() );
    }
}
