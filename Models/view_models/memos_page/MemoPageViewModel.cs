using SaYMemos.Models.form_classes;
using SaYMemos.Models.view_models.memos;

namespace SaYMemos.Models.view_models.memos_page
{
    public record class MemoPageViewModel(
        MemoPreviewViewModel[] package,
        MemoFilterForm filer,
        MemoSortOptionsForm sorting
        )
    {
        public static MemoPageViewModel Default(MemoPreviewViewModel[] memos) =>
             new(memos, MemoFilterForm.Default(), new(SortOptions.Date, true));
    }
}
