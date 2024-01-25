﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using MimeKit;
using SaYMemos.Models.view_models.memos;
using System.IO;
using System.Text;
namespace SaYMemos.Models.tag_helpers
{
    [HtmlTargetElement("memo")]
    public class Memo : TagHelper
    {
        public OneMemoViewModel MemoContent { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "memo");
            output.TagMode = TagMode.StartTagAndEndTag;

            var content = new StringBuilder();
            content.Append(RenderTopInfo());
            content.Append(RenderComment());
            content.Append(RenderImage());
            content.Append(RenderInteractions());

            output.Content.SetHtmlContent(content.ToString());
        }

        private string RenderTopInfo()
        {
            return $"<div class='memo-top-info'>" +
                   $"<div>" +
                   $"<img src='{MemoContent.authorProfilePicture.EncodeHtml()}' alt='Profile Picture'/>" +
                   $"<label class='memo-author'>{MemoContent.authorNickName.EncodeHtml()}</label>" +
                   $"</div>" +
                   $"<label class='memo-date'>{MemoContent.creationDate.EncodeHtml()}</label>" +
                   $"</div>";
        }

        private string RenderComment()
        {
            return $"<div class='memo-comment'>{MemoContent.authorComment.EncodeHtml()}</div>";
        }

        private string RenderImage()
        {
            return !string.IsNullOrEmpty(MemoContent.imagePath) ?
                   $"<div class='memo-image'><img src='{MemoContent.imagePath.EncodeHtml()}' alt='Memo Image'/></div>" :
                   string.Empty;
        }

        private string RenderInteractions()
        {
            var interactionsBuilder = new StringBuilder("<div class='memo-interactions'>");

            if (MemoContent.isLiked)
                interactionsBuilder.Append($"<span hx-trigger='click' hx-post='MemoInteraction/LikePressed' hx-target='this' class='memo-like-pressed' hx-vals='{{\"memoId\": \"{MemoContent.id}\"}}' >{LikeIconPressed}</span>");
            else
                interactionsBuilder.Append($"<span hx-trigger='click' hx-post='MemoInteraction/LikePressed' hx-target='this' class='memo-icon' hx-vals='{{\"memoId\": \"{MemoContent.id}\"}}' >{LikeIcon}</span>");

            if (MemoContent.areCommentsAvaliable)
                interactionsBuilder.Append($"<span class='memo-icon'>{CommentIcon}</span>");

            interactionsBuilder.Append($"<span class='memo-icon'>{ShareIcon}</span>");
            interactionsBuilder.Append("</div>");


            if (MemoContent.areCommentsAvaliable)
                interactionsBuilder.Append("<div class='memo-add-comment'><input type='text' placeholder='Add a comment...'><button type='submit'>Post</button></div>");
            return interactionsBuilder.ToString();
        }

        private string LikeIcon =
            "<svg viewBox=\"0 0 512 512\"><path d=\"M225.8 468.2l-2.5-2.3L48.1 303.2C17.4 274.7 0 234.7 0 192.8v-3.3c0-70.4 50-130.8 119.2-144C158.6 37.9 198.9 47 231 69.6c9 6.4 17.4 13.8 25 22.3c4.2-4.8 8.7-9.2 13.5-13.3c3.7-3.2 7.5-6.2 11.5-9c0 0 0 0 0 0C313.1 47 353.4 37.9 392.8 45.4C462 58.6 512 119.1 512 189.5v3.3c0 41.9-17.4 81.9-48.1 110.4L288.7 465.9l-2.5 2.3c-8.2 7.6-19 11.9-30.2 11.9s-22-4.2-30.2-11.9zM239.1 145c-.4-.3-.7-.7-1-1.1l-17.8-20c0 0-.1-.1-.1-.1c0 0 0 0 0 0c-23.1-25.9-58-37.7-92-31.2C81.6 101.5 48 142.1 48 189.5v3.3c0 28.5 11.9 55.8 32.8 75.2L256 430.7 431.2 268c20.9-19.4 32.8-46.7 32.8-75.2v-3.3c0-47.3-33.6-88-80.1-96.9c-34-6.5-69 5.4-92 31.2c0 0 0 0-.1 .1s0 0-.1 .1l-17.8 20c-.3 .4-.7 .7-1 1.1c-4.5 4.5-10.6 7-16.9 7s-12.4-2.5-16.9-7z\"/></svg>";
        private string LikeIconPressed=
             "<svg viewBox=\"0 0 512 512\"><path d=\"M47.6 300.4L228.3 469.1c7.5 7 17.4 10.9 27.7 10.9s20.2-3.9 27.7-10.9L464.4 300.4c30.4-28.3 47.6-68 47.6-109.5v-5.8c0-69.9-50.5-129.5-119.4-141C347 36.5 300.6 51.4 268 84L256 96 244 84c-32.6-32.6-79-47.5-124.6-39.9C50.5 55.6 0 115.2 0 185.1v5.8c0 41.5 17.2 81.2 47.6 109.5z\"/></svg>";
        private string CommentIcon =
            "<svg viewBox=\"0 0 512 512\"><path d=\"M123.6 391.3c12.9-9.4 29.6-11.8 44.6-6.4c26.5 9.6 56.2 15.1 87.8 15.1c124.7 0 208-80.5 208-160s-83.3-160-208-160S48 160.5 48 240c0 32 12.4 62.8 35.7 89.2c8.6 9.7 12.8 22.5 11.8 35.5c-1.4 18.1-5.7 34.7-11.3 49.4c17-7.9 31.1-16.7 39.4-22.7zM21.2 431.9c1.8-2.7 3.5-5.4 5.1-8.1c10-16.6 19.5-38.4 21.4-62.9C17.7 326.8 0 285.1 0 240C0 125.1 114.6 32 256 32s256 93.1 256 208s-114.6 208-256 208c-37.1 0-72.3-6.4-104.1-17.9c-11.9 8.7-31.3 20.6-54.3 30.6c-15.1 6.6-32.3 12.6-50.1 16.1c-.8 .2-1.6 .3-2.4 .5c-4.4 .8-8.7 1.5-13.2 1.9c-.2 0-.5 .1-.7 .1c-5.1 .5-10.2 .8-15.3 .8c-6.5 0-12.3-3.9-14.8-9.9c-2.5-6-1.1-12.8 3.4-17.4c4.1-4.2 7.8-8.7 11.3-13.5c1.7-2.3 3.3-4.6 4.8-6.9c.1-.2 .2-.3 .3-.5z\"/></svg>";
        private string ShareIcon = "<svg style='transform: scaleX(-1);' viewBox=\"0 0 22 22\"><path d=\"M11 7.05V4a1 1 0 0 0-1-1 1 1 0 0 0-.7.29l-7 7a1 1 0 0 0 0 1.42l7 7A1 1 0 0 0 11 18v-3.1h.85a10.89 10.89 0 0 1 8.36 3.72 1 1 0 0 0 1.11.35A1 1 0 0 0 22 18c0-9.12-8.08-10.68-11-10.95zm.85 5.83a14.74 14.74 0 0 0-2 .13A1 1 0 0 0 9 14v1.59L4.42 11 9 6.41V8a1 1 0 0 0 1 1c.91 0 8.11.2 9.67 6.43a13.07 13.07 0 0 0-7.82-2.55z\"/></svg>";

    }
}