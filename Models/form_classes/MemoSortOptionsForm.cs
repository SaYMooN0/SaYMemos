using MimeKit.Cryptography;

namespace SaYMemos.Models.form_classes
{
    public record class MemoSortOptionsForm(SortOptions sortType, bool isDescending)
    {
        public static MemoSortOptionsForm Default() =>
            new(SortOptions.Date, true);
    }
    public enum SortOptions
    {
        Date,
        Likes,
        Comments
    }
}
