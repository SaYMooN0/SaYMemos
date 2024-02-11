using SaYMemos.Models.form_classes;
namespace SaYMemos.Models.view_models.memos_page
{
    public record class MemoPageViewModel(
        MemoFilterForm filer,
        MemoSortOptionsForm sorting
        )
    {
        public static MemoPageViewModel Default() =>
             new(MemoFilterForm.Default(), new(SortOptions.Date, true));
    }
}
