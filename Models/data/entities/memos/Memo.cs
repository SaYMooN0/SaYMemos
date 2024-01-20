using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.data.entities.memos
{
    public record class Memo(
        Guid id,
        long authorId,
        string authorComment,
        string? imagePath,
        string? hashtagsString,
        DateTime creationTime

        )
    {
        public string[] Hashtags => string.IsNullOrWhiteSpace(hashtagsString) ? Array.Empty<string>() : hashtagsString.Split(',');
        virtual public User Author { get; init; }
        public static Memo CreateNew(long authorId, string authorComment, string? imagePath = null, params long[] hashtags)
            => new(new(), authorId, authorComment, imagePath,string.Join(',',hashtags) ,DateTime.Now);


    }
}
