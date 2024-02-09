namespace SaYMemos.Models.form_classes
{
    public record class MemoSortOptionsForm(SortOptions sortType, bool isDescending)
    {
    }
    public enum SortOptions
    {
        Date,
        Likes,
        Comments
    }
}
