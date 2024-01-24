using SaYMemos.Models.data.entities.users;
using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.comments
{
    public record class CommentRating(
        [property: Key] Guid id, 
        Guid commentId,
        long userId,
        bool isUp)
    {
        public virtual Comment Comment { get; set; }
        public virtual User User { get; set; }
    }

}
