using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.data.entities.memos
{
    public record class Memo(
        Guid id,
        long authorId,
        string authorComment,
        string images,
        DateTime creationTime
        )
    {
        virtual public User Author { get; set; }
        public static Memo CreateNew(long authorId, string authorComment, params long[] imageIds)
            => new(new(), authorId, authorComment, string.Join(',', imageIds), DateTime.Now);
        public long[] GetImageIds => images.Split(',').Select(long.Parse).ToArray();

    }
}
