using System.Text.Encodings.Web;

namespace SaYMemos.Models.tag_helpers
{
    public static class StringExtensions
    {
        public static string EncodeHtml(this string input) =>
            HtmlEncoder.Default.Encode(input);
    }
}
