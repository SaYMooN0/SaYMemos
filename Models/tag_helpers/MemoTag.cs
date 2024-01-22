using Microsoft.AspNetCore.Razor.TagHelpers;
using SaYMemos.Models.view_models;
using System.Text;
using System.Text.Encodings.Web;

namespace SaYMemos.Models.tag_helpers
{
    [HtmlTargetElement("memotag")]
    public class MemoTag : TagHelper
    {
        public OneMemoViewModel MemoContent { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "memo");
            output.TagMode = TagMode.StartTagAndEndTag;

            var content = new StringBuilder();
            content.AppendFormat("<div class='memo-author'><img src='{0}' alt='Profile Picture'/><span>{1}</span></div>",
                HtmlEncoder.Default.Encode(MemoContent.authorProfilePicture),
                HtmlEncoder.Default.Encode(MemoContent.authorNickName));

            content.AppendFormat("<div class='memo-comment'>{0}</div>",
                HtmlEncoder.Default.Encode(MemoContent.authorComment));

            if (!string.IsNullOrEmpty(MemoContent.imagePath))
            {
                content.AppendFormat("<div class='memo-image'><img src='{0}' alt='Memo Image'/></div>",
                    HtmlEncoder.Default.Encode(MemoContent.imagePath));
            }

            content.AppendFormat("<div class='memo-date'>{0}</div>",
                HtmlEncoder.Default.Encode(MemoContent.creationDate));

            if (MemoContent.tags != null && MemoContent.tags.Length > 0)
            {
                content.Append("<div class='memo-tags'>");
                foreach (var tag in MemoContent.tags)
                {
                    content.AppendFormat("<span class='tag'>{0}</span>",
                        HtmlEncoder.Default.Encode(tag));
                }
                content.Append("</div>");
            }

            output.Content.SetHtmlContent(content.ToString());
        }
    }
}
