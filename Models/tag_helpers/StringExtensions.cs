using System.Text.Encodings.Web;

namespace SaYMemos.Models.tag_helpers
{
    public static class StringExtensions
    {
        public static string EncodeHtml(this string input) =>
            HtmlEncoder.Default.Encode(input);
        public static string OpenMemoDialogOnClick(Guid id)  => 
            $"hx-target='#memo-dialog' hx-swap='innerHTML' hx-vals='{{\"memoId\": \"{id}\"}}' hx-trigger='click' hx-post='MemoInteraction/RenderAllMemoInfo'";
    }
}
