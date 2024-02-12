using SaYMemos.Models.form_classes;
namespace SaYMemos.Models.view_models.memos_page
{
    public record class MemoPageViewModel(
        MemoFilterForm filter,
        MemoSortOptionsForm sorting
        )
    {
        public static MemoPageViewModel Default() =>
             new(MemoFilterForm.Default(), new(SortTypes.Date, true));
    }
}
