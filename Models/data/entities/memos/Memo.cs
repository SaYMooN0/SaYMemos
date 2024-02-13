using SaYMemos.Models.data.entities.comments;
using SaYMemos.Models.data.entities.users;
using System.ComponentModel.DataAnnotations;
namespace SaYMemos.Models.data.entities.memos
{
    public record class Memo(
        [property: Key] Guid id,
        long authorId,
        string authorComment,
        string? imagePath,
        bool areCommentsAvailable,
        DateTime creationTime)
    {
        public const int PackageCount = 10;
        virtual public User Author { get; init; }
        virtual public ICollection<MemoTag> Tags { get; init; } = new HashSet<MemoTag>();
        public virtual ICollection<MemoLike> Likes { get; set; } = new HashSet<MemoLike>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public static Memo CreateNew(long authorId, string authorComment, bool areCommentsAvailable, string? imagePath = null, params MemoTag[] tags)
            => new(new(), authorId, authorComment, imagePath, areCommentsAvailable, DateTime.UtcNow) { Tags = tags.ToList() };
    }
    public delegate IQueryable<Memo> MemoFilterFunc(IQueryable<Memo> query);
}
