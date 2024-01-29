using Microsoft.AspNetCore.Razor.TagHelpers;
namespace SaYMemos.Models.tag_helpers
{
    [HtmlTargetElement("memo-tag")]
    public class MemoTagTH : TagHelper
    {
        [HtmlAttributeName("tagname")]
        public string tagName { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("class", "memo-tag");
            output.Attributes.SetAttribute("href", "memos?tag="+tagName);
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent( StringExtensions.EncodeHtml('#' + tagName) );
        }
    }
}
