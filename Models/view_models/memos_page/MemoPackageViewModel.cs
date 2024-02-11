using SaYMemos.Models.view_models.memos;

namespace SaYMemos.Models.view_models.memos_page
{
    public record class MemoPackageViewModel(
        MemoPreviewViewModel[] memos,
        int nextNumber
    );


}
