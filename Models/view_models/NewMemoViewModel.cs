

namespace SaYMemos.Models.view_models
{
    public class NewMemoViewModel
    {
        public string AuthorComment {  get; private set; } =string.Empty;
        public List<string> ImagePaths { get; private set; }= new();

    }
}
