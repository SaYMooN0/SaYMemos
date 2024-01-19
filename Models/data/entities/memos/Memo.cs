using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.data.entities.memos
{
    public record class Memo(
        Guid id,
        long authorId,
        string authorComment,
        string? imagePath,
        DateTime creationTime
        )
    {
        virtual public User Author { get; init; }
        //public static Memo CreateNew(long authorId, string authorComment, string imagePath)
        //    => new (new(), authorId, authorComment, imagePath, DateTime.Now);
       

    }
}
