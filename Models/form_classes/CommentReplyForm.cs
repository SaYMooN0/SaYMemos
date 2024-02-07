using SaYMemos.Models.data.entities.comments;
using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.form_classes
{
    public record class CommentReplyForm(
        string userProfilePicture,
        string userNickname,
        string userToReply,
        string messageToReplyPreview,
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
        public static CommentReplyForm CreateNew(User replyingUser, Comment parentComment) =>
           new(replyingUser.ProfilePicturePath,replyingUser.Nickname, parentComment.Author.Nickname,parentComment.Text.Length<20? parentComment.Text: parentComment.Text.Substring(0,18)+"...", parentComment.Id.ToString(), string.Empty, string.Empty);

    }

}
