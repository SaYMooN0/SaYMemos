using SaYMemos.Models.data.entities.memos;
namespace SaYMemos.Models.form_classes
{
    public record MemoCreationForm(
        string authorComment,
        IFormFile image,
        List<string> hashtags
        )
    {
        public MemoCreationForm Validate()
        {
            throw new NotImplementedException();
        }
        //public static Memo ParseToMemo();
    }
}
