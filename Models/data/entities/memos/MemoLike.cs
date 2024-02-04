using SaYMemos.Models.data.entities.users;
using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.memos
{
    public class MemoLike
    {
        [Key]
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public Guid MemoId { get; set; }
        public DateTime DateTime { get; set; }
        public static MemoLike CreateNew(Guid memoId, long userId)
        {
            return new MemoLike
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                MemoId = memoId,
                DateTime = DateTime.UtcNow
            };
        }
        public virtual User User { get; set; }
        public virtual Memo Memo { get; set; }
        
    }

}
