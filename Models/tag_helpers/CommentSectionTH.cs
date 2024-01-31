using Microsoft.AspNetCore.Razor.TagHelpers;
using SaYMemos.Models.view_models.memos;
using System.Text;

namespace SaYMemos.Models.tag_helpers
{
    [HtmlTargetElement("comment-section")]
    public class CommentSectionTH : TagHelper
    {
        public CommentViewModel Comment { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "comment");
            output.TagMode = TagMode.StartTagAndEndTag;

            var content = new StringBuilder();
            content.Append(RenderComment(Comment, true));

            output.Content.SetHtmlContent(content.ToString());
        }

        private string RenderComment(CommentViewModel comment, bool isParent)
        {
            var commentClass = isParent ? "comment-parent" : "comment-child";
            var encodedText = comment.text.EncodeHtml();
            var encodedNickname = comment.authorNickname.EncodeHtml();
            var encodedProfilePicture = comment.authorProfilePicture.EncodeHtml();

            var commentHtml = new StringBuilder();
            commentHtml.AppendFormat(
                "<div class='{0}'>" +
                "<img src='{1}' alt='Author Picture' class='comment-author-picture'>" +
                "<span class='comment-author-nickname'>{2}</span>" +
                "<p class='comment-text'>{3}</p>" +
                "<span class='comment-rating'>Rating: {4}</span>",
                commentClass, encodedProfilePicture, encodedNickname, encodedText, comment.totalRating
            );
            //rate and answer buttons
            if (comment.childComments.Any())
            {
                foreach (var childComment in comment.childComments)
                {
                    commentHtml.Append(RenderComment(childComment, false));
                }
            }

            commentHtml.Append("</div>");
          
            return commentHtml.ToString();
        }
    }
}
