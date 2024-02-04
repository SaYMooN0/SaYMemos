using SaYMemos.Models.data.entities.memos;
using SaYMemos.Models.data.entities.users;
using System.ComponentModel.DataAnnotations;
namespace SaYMemos.Models.data.entities.comments
{
    public class Comment
    {


        [Key]
        public Guid Id { get;private set; }
        public Guid MemoId { get; private  set; }
        public long AuthorId { get; private set; }
        public Guid? ParentCommentId { get; private set; }
        public string Text { get; private set; }
        public DateTime LeavingDate { get; private set; }
        public Comment(Guid id, Guid memoId, long authorId, Guid? parentCommentId, string text, DateTime leavingDate)
        {
            Id = id;
            MemoId = memoId;
            AuthorId = authorId;
            ParentCommentId = parentCommentId;
            Text = text;
            LeavingDate = leavingDate;
        }
        public static Comment CreateNew(Guid memoId, long authorId, Guid? parentCommentId, string text) =>
            new(new(), memoId, authorId, parentCommentId, text, DateTime.UtcNow);
        public virtual Memo Memo { get; set; }
        public virtual User Author { get; set; }
        public virtual Comment ParentComment { get; set; }
        public virtual ICollection<Comment> ChildComments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<CommentRating> CommentRatings { get; set; } = new HashSet<CommentRating>();

        public int CountRating()
        {
            return CommentRatings.Count(r => r.IsUp) - CommentRatings.Count(r => !r.IsUp);
        }
    }
}
