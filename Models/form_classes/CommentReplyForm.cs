using SaYMemos.Models.data.entities.comments;

namespace SaYMemos.Models.form_classes
{
    public record class CommentReplyForm(
        string userProfilePicture,
        string userToReply,
        string parentCommentId,
        string text,
        string errorLine
        )

    {
        public Guid? ParentCommentGuid() =>
            Guid.TryParse(parentCommentId, out Guid commentGuid) ? commentGuid : null;
        public bool AnyError() =>
           !string.IsNullOrEmpty(errorLine);
        public CommentReplyForm Validate() =>
            (ParentCommentGuid() is null) ? this with { errorLine = "Unknown comment" } :
            string.IsNullOrWhiteSpace(text) ? this with { errorLine = "You cannot leave an empty comment" } :
            this with { errorLine = string.Empty };
        public static CommentReplyForm CreateNew(string profilePicture, Comment parentComment) =>
           new(profilePicture, parentComment.Author.Nickname, parentComment.Id.ToString(), string.Empty, string.Empty);

    }

}
