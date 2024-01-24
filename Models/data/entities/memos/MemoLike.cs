using SaYMemos.Models.data.entities.users;
using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.memos
{
    public record class MemoLike(
        [property: Key] Guid id,
        long userId,
        Guid memoGuid,
        DateTime dateTime
        )
    {
        virtual public User User { get; init; }
        virtual public Memo Memo{ get; init; }
    }
}
