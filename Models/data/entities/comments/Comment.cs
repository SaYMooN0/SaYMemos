using SaYMemos.Models.data.entities.memos;
using SaYMemos.Models.data.entities.users;
using System.ComponentModel.DataAnnotations;
namespace SaYMemos.Models.data.entities.comments
{
    public record class Comment(
    [property: Key] Guid id,
    Guid memoId,
    long authorId,
    Guid? parentCommentId,
    string text)
    {
        public virtual Memo Memo { get; init; }
        public virtual User Author { get; init; }
        public virtual Comment ParentComment { get; init; }
        public virtual ICollection<Comment> ChildComments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<CommentRating> Ratings { get; set; } = new HashSet<CommentRating>();
        public int CountRating()=>
            Ratings.Count(r => r.isUp) - Ratings.Count(r => !r.isUp);
        
    }

}
