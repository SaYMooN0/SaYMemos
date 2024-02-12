using MimeKit.Cryptography;

namespace SaYMemos.Models.form_classes
{
    public record class MemoSortOptionsForm(SortTypes sortType, bool isDescending)
    {
        public static MemoSortOptionsForm Default() =>
            new(SortTypes.Date, true);
    }
    public enum SortTypes
    {
        Date = 0,
        Likes = 1,
        Comments = 2
    }
}
