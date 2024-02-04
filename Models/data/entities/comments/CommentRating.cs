using SaYMemos.Models.data.entities.users;
using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.comments
{
    public class CommentRating
    {


        [Key]
        public Guid Id { get; private set; }
        public Guid CommentId { get; private set; }
        public long UserId { get; private set; }
        public bool IsUp { get; private set; }
        public void ChangeIsUp()=>
            IsUp = !IsUp;
        public static CommentRating CreateNew(Guid commentId, long userId, bool isUp)
        {
            return new CommentRating
            {
                Id = Guid.NewGuid(),
                CommentId = commentId,
                UserId = userId,
                IsUp = isUp
            };
        }
        public virtual Comment Comment { get; set; }
        public virtual User User { get; set; }
    }

}
