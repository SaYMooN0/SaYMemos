using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SaYMemos.Models.view_models.memos;
using System.Text;
using System.Text.Json;

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

            var commentHtml = new StringBuilder();
            commentHtml.AppendFormat(
                "<div class='{0}'>" +
                "<div class='comment-meta-info'>" +
                    "<div class='comment-meta-left'>" +
                    "<img src='{1}' alt='Author Picture' class='comment-author-picture'>" +
                    "<div>" +
                        "<span class='comment-author-nickname'>{2}</span>" +
                        "<span class='comment-date'>{3}</span>" +
                    "</div>" +
                "</div>" +
                "<div class='comment-meta-right'>" +
                    "<span class='comment-rating'>{4}</span>" +
                    "<div class='comment-rating-buttons'>{5}</div>" +
                "</div>" +
                "</div>" +
                "<p class='comment-text'>{6}</p>" +
                "<button class='answer-button'>Answer</button>",
                commentClass,
                comment.authorProfilePicture.EncodeHtml(),
                comment.authorNickname.EncodeHtml(),
                comment.leavingDate.EncodeHtml(),
                comment.totalRating,
                RenderVoteButtons(comment),
                encodedText
            );
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
        private string RenderVoteButtons(CommentViewModel comment)
        {

            string voteButtonsHtml = @"<div class=""comment-rating-buttons"">";

            if (!comment.isRated || comment.isUp is null)
            {
                voteButtonsHtml += $@"
                    <span class=""comment-rating-button"" {hxAtrributes()} {hxVals(true, comment)}>
                        <svg viewBox=""0 0 24 24"">
                            {iconPath}
                        </svg>
                    </span>
                    <span class=""comment-rating-button"" {hxAtrributes()} {hxVals(false, comment)}>
                        <svg viewBox=""0 0 24 24"" style='transform: scaleY(-1);'>
                            {iconPath}
                        </svg>
                    </span>
                ";
            }
            else if (comment.isUp == true)
            {
                voteButtonsHtml += $@"
                    <span class=""comment-rating-button"" {hxAtrributes()} {hxVals(true, comment)}>
                        <svg viewBox=""0 0 24 24"">
                            {iconFilledPath}
                        </svg>
                    </span>
                    <span class=""comment-rating-button"" {hxAtrributes()} {hxVals(false, comment)}>
                        <svg viewBox=""0 0 24 24"" style='transform: scaleY(-1);'>
                            {iconPath}
                        </svg>
                    </span>
                ";
            }
            else
            {
                voteButtonsHtml += $@"
                    <span class=""comment-rating-button"" {hxAtrributes()} {hxVals(true,comment)}>
                        <svg viewBox=""0 0 24 24"">
                            {iconPath}
                        </svg>
                    </span>
                    <span class=""comment-rating-button"" {hxAtrributes()} {hxVals(false,comment)}>
                        <svg viewBox=""0 0 24 24"" style='transform: scaleY(-1);'>
                            {iconFilledPath}
                        </svg>
                    </span>
                ";
            }

            voteButtonsHtml +="</div>";

            return voteButtonsHtml;
        }
        private HtmlString hxVals(bool isUp, CommentViewModel comment)
        {
            var vals = new Dictionary<object, object>
            {
                ["memoId"] = comment.memoId,
                ["commentId"] = comment.id,
                ["isUp"] = isUp
            };
            return new HtmlString($"hx-vals='{JsonSerializer.Serialize(vals)}'");
        }
        private HtmlString hxAtrributes() =>
            new HtmlString("hx-redirect='/authorization' hx-trigger='click' hx-swap='outerHtml' hx-post='/MemoInteraction/commentVote' hx-target='comment-rating-buttons'");

        private HtmlString iconFilledPath = new HtmlString(@"<path d=""M4 14h4v7a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1v-7h4a1.001 1.001 0 0 0 .781-1.625l-8-10c-.381-.475-1.181-.475-1.562 0l-8 10A1.001 1.001 0 0 0 4 14z"" />");
        private HtmlString iconPath = new HtmlString(@"<path d=""M4 14h4v7a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1v-7h4a1.001 1.001 0 0 0 .781-1.625l-8-10c-.381-.475-1.181-.475-1.562 0l-8 10A1.001 1.001 0 0 0 4 14z"" />");
    }
}
