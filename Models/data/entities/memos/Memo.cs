using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.data.entities.memos
{
    public record class Memo(
    Guid id,
    long authorId,
    string authorComment,
    string? imagePath,
    DateTime creationTime)
    {
        virtual public User Author { get; init; }
        virtual public ICollection<MemoTag> Tags { get; init; } = new HashSet<MemoTag>();

        public static Memo CreateNew(long authorId, string authorComment, string? imagePath = null, params MemoTag[] tags)
            => new(new(), authorId, authorComment, imagePath, DateTime.UtcNow) { Tags = tags.ToList() };
    }

}
