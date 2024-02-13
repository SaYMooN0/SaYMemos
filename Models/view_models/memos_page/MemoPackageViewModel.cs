using SaYMemos.Models.data.entities.memos;
using SaYMemos.Models.view_models.memos;

namespace SaYMemos.Models.view_models.memos_page
{
    public record class MemoPackageViewModel(
        IEnumerable<MemoPreviewViewModel> memos,
        int nextNumber
    )
    {
        public static MemoPackageViewModel FromMemosForUnauthorized(IEnumerable<Memo> memos, int nextNumber) =>
            new(memos.Select(m => MemoPreviewViewModel.FromMemo(m)), nextNumber);

    }


}
