using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SaYMemos.Models.tag_helpers;

namespace SaYMemos.Models.tag_helpers
{
    [HtmlTargetElement("add-filter-tag")]
    public class AddTagTH : TagHelper
    {
        [HtmlAttributeName("tagname")]
        public string tagName { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "filter-add-tag");
            output.TagMode = TagMode.StartTagAndEndTag;
            HtmlString content = new(
                $"<label>{StringExtensions.EncodeHtml('#' + tagName)}</label>" +
                "<button hx-trigger='click' hx-post='' >Add</button>");
            output.Content.SetHtmlContent(content);
        }
    }
}