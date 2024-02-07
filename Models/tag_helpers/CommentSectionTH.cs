using Microsoft.AspNetCore.Html;
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
            content.Append(RenderComment(Comment, Comment.isParent));

            output.Content.SetHtmlContent(content.ToString());
        }

        private string RenderComment(CommentViewModel comment, bool isParent)
        {
            var commentClass = isParent ? "comment-parent" : "comment-child";
            var commentHtml = new StringBuilder();

            commentHtml.Append($"<div class='{commentClass}'>")
                       .Append(RenderMetaInfo(comment))
                       .Append($"<p class='comment-text'>{comment.text.EncodeHtml()}</p>")
                       .Append(RenderBottomInfo(comment))
                       .Append(RenderReplyButton(comment.id.ToString()));

            foreach (var childComment in comment.childComments)
            {
                commentHtml.Append(RenderComment(childComment, false));
            }
            commentHtml.Append("</div>");
            return commentHtml.ToString();
        }

        private string RenderMetaInfo(CommentViewModel comment)=>
            $"<div class='comment-meta-info'>" +
                   $"<div class='comment-meta-left'>" +
                   $"<img src='{comment.authorProfilePicture.EncodeHtml()}' alt='Author Picture' class='comment-author-picture'>" +
                   "<div>" +
                   $"<span class='comment-author-nickname'>{comment.authorNickname.EncodeHtml()}</span>" +
                   $"<span class='comment-date'>{comment.leavingDate.EncodeHtml()}</span>" +
                   "</div>" +
                   "</div>" +
                   $"<div class='comment-meta-right' id='mr-{comment.id}'>" +
                   $"<span class='comment-rating'>{comment.totalRating}</span>" +
                   RenderVoteButtons(comment) +
                   "</div>" +
                   "</div>";

        private string RenderBottomInfo(CommentViewModel comment)=>
            $"<div class='comment-bottom-info' id='bi-{comment.id}'>" +
            $"<label class='comment-bottom-label'>Replies: {comment.childComments.Length}</label>" +
            $"<label class='comment-bottom-label'>Ratings: {comment.ratingsCount}</label>" +
            "</div>";

        private string RenderReplyButton(string commentId)=>
            $"<button class='answer-button' hx-trigger='click' hx-swap='outerHTML' hx-target='this' hx-redirect='/authorization' hx-post='/MemoInteraction/RenderCommentReplyForm' hx-vals='{{\"commentId\": \"{commentId}\"}}'>Reply</button>";
       
        private string RenderVoteButtons(CommentViewModel comment)
        {

            string voteButtonsHtml = @"<div class=""comment-rating-buttons"">";

            if (!comment.isRated || comment.isUp is null)
            {
                voteButtonsHtml += $@"
                    <span class=""comment-rating-button"" {hxAtrributes(true, comment)} >
                        <svg viewBox=""0 0 24 24"">
                            {iconPath}
                        </svg>
                    </span>
                    <span class=""comment-rating-button""  {hxAtrributes(false, comment)} >
                        <svg viewBox=""0 0 24 24"" style='transform: scaleY(-1);'>
                            {iconPath}
                        </svg>
                    </span>
                ";
            }
            else if (comment.isUp == true)
            {
                voteButtonsHtml += $@"
                    <span class=""comment-rating-button"" {hxAtrributes(true, comment)} >
                        <svg viewBox=""0 0 24 24"">
                            {iconFilledPath}
                        </svg>
                    </span>
                    <span class=""comment-rating-button"" {hxAtrributes(false, comment)} >
                        <svg viewBox=""0 0 24 24"" style='transform: scaleY(-1);'>
                            {iconPath}
                        </svg>
                    </span>
                ";
            }
            else
            {
                voteButtonsHtml += $@"
                    <span class=""comment-rating-button""  {hxAtrributes(true, comment)} >
                        <svg viewBox=""0 0 24 24"">
                            {iconPath}
                        </svg>
                    </span>
                    <span class=""comment-rating-button""  {hxAtrributes(false, comment)} >
                        <svg viewBox=""0 0 24 24"" style='transform: scaleY(-1);'>
                            {iconFilledPath}
                        </svg>
                    </span>
                ";
            }

            voteButtonsHtml += "</div>";

            return voteButtonsHtml;
        }
        private string hxAtrributes(bool isUp, CommentViewModel comment)
        {
            string jsonParams = $"{{\"commentId\": \"{comment.id}\", \"isUp\": {isUp.ToString().ToLower()}}}";
            return $"hx-vals='js:{jsonParams}' hx-redirect='/authorization' hx-trigger='click' hx-swap='outerHTML' hx-post='/MemoInteraction/RateComment' hx-target='#mr-{comment.id}' ";
        }

        private HtmlString iconFilledPath = new(@"<path d=""M4 14h4v7a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1v-7h4a1.001 1.001 0 0 0 .781-1.625l-8-10c-.381-.475-1.181-.475-1.562 0l-8 10A1.001 1.001 0 0 0 4 14z"" />");
        private HtmlString iconPath = new(@"<path d=""M12.781 2.375c-.381-.475-1.181-.475-1.562 0l-8 10A1.001 1.001 0 0 0 4 14h4v7a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1v-7h4a1.001 1.001 0 0 0 .781-1.625l-8-10zM15 12h-1v8h-4v-8H6.081L12 4.601 17.919 12H15z""/>");
    }
}
