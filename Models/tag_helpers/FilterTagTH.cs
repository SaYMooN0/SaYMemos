
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SaYMemos.Models.tag_helpers
{
    [HtmlTargetElement("filter-tag")]
    public class FilterTagTH : TagHelper
    {
        [HtmlAttributeName("tagname")]
        public string tagName { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "filter-added-tag");
            output.TagMode = TagMode.StartTagAndEndTag;
            HtmlString content = new(
                $"<label>{StringExtensions.EncodeHtml('#' + tagName)}</label>" +
                $"<input type='hidden' name='tags' value='{tagName}'/>" +
                $"<button hx-trigger='click' hx-swap='delete' hx-get='/ok' hx-target='closest .filter-added-tag'>&#10005;</button>");
            output.Content.SetHtmlContent(content);
        }
    }
}