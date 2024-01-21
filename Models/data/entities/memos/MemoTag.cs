using SaYMemos.Models.data.entities.memos;
using System.ComponentModel.DataAnnotations;
namespace SaYMemos.Models.data.entities.users
{
    public record class MemoTag(
     [property: Key] long Id,
     string Value)
    {
        public virtual ICollection<Memo> Memos { get; init; } = new HashSet<Memo>();
    }

}