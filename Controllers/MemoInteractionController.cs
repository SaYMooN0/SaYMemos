using Microsoft.AspNetCore.Mvc;
using SaYMemos.Controllers.Helpers;
using SaYMemos.Models.data.entities.comments;
using SaYMemos.Models.data.entities.memos;
using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.view_models.memos;
using SaYMemos.Services.interfaces;
using System;
using System.ComponentModel.Design;
using ILogger = SaYMemos.Services.interfaces.ILogger;

namespace SaYMemos.Controllers
{
    public class MemoInteractionController : Controller
    {
        IDatabase _db { get; init; }
        ILogger _logger { get; init; }
        IEncryptor _enc { get; init; }
        public MemoInteractionController(IDatabase db, ILogger logger, IEncryptor enc)
        {
            _db = db;
            _logger = logger;
            _enc = enc;
        }

        [HttpPost]
        public async Task<IActionResult> LikePressed(string memoId)
        {

            if (!Guid.TryParse(memoId, out Guid parsedMemoId))
                return BadRequest("Invalid Memo ID format.");

            long userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return this.HxUnauthorized();

            User? user = await _db.GetUserById(userId);
            if (user is null)
                return this.HxUnauthorized();

            await _db.UpdateLastLoginDateForUser(user);

            bool isLikedAfter = await _db.ChangeLikeState(user, parsedMemoId);
            return PartialView(viewName: "LikeIcon", new MemoLikeViewModel(isLikedAfter, memoId));
        }


        [HttpPost]
        public async Task<IActionResult> LeaveComment(string memoId, string commentText)
        {

            if (!Guid.TryParse(memoId, out Guid parsedMemoId))
                return BadRequest("Invalid Memo ID format");
            if (string.IsNullOrEmpty(commentText))
                return BadRequest("Invalid comment");

            long userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return this.HxUnauthorized();
            User? user = await _db.GetUserById(userId);
            if (user is null)
                return this.HxUnauthorized();

            await _db.UpdateLastLoginDateForUser(userId);


            Comment? addedComment = await _db.AddCommentToMemo(parsedMemoId, commentText, user.Id);
            if (addedComment is null)
                return PartialView(viewName: "CommentForm", CommentFormViewModel.Error(parsedMemoId));

            return PartialView(viewName: "CommentForm", CommentFormViewModel.Success(addedComment.Id, parsedMemoId));
        }
        [HttpPost]
        public async Task<IActionResult> RenderAddedComment(string commentId)
        {

            if (!Guid.TryParse(commentId, out Guid commentGuid))
                return BadRequest("Invalid ID");
            Comment? comment = await _db.GetCommentById(commentGuid);
            if (comment is null)
                return BadRequest("Unknown comment");

            long userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return PartialView(viewName: "AddedComment", CommentViewModel.FromCommentForUnauthorized(comment));

            User? user = await _db.GetUserById(userId);
            if (user is null)
                return PartialView(viewName: "AddedComment", CommentViewModel.FromCommentForUnauthorized(comment));

            await _db.UpdateLastLoginDateForUser(userId);

            return PartialView(viewName: "AddedComment", CommentViewModel.FromCommentForUser(comment, user));
        }

        [HttpPost]
        public async Task<IActionResult> RenderAllMemoInfo(string memoId)
        {
            if (!Guid.TryParse(memoId, out Guid parsedMemoId))
                return BadRequest("Invalid Memo ID format.");

            Memo? memo = await _db.GetMemoById(parsedMemoId);
            if (memo is null)
                return BadRequest("Unknown Memo");

            long userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return PartialView(viewName: "FullMemoInfo", model: MemoFullInfoViewModel.FromMemoForUnauthorized(memo));

            User? user = await _db.GetUserById(userId);
            if (user is null)
                return PartialView(viewName: "FullMemoInfo", model: MemoFullInfoViewModel.FromMemoForUnauthorized(memo));

            await _db.UpdateLastLoginDateForUser(userId);
            return PartialView(viewName: "FullMemoInfo", model: MemoFullInfoViewModel.FromMemoForUser(memo, user));
        }
        [HttpPost]
        public async Task<IActionResult> RenderTags(string memoId)
        {
            if (!Guid.TryParse(memoId, out Guid parsedMemoId))
                return BadRequest("Invalid Memo ID format.");

            Memo? memo = await _db.GetMemoById(parsedMemoId);
            if (memo is null)
                return BadRequest("Unknown Memo");

            return PartialView(viewName: "TagsZone", model: memo.Tags.Select(t => t.Value));
        }
        [HttpPost]
        public async Task<IActionResult> RateComment(string commentId, bool isUp)
        {
            if (!Guid.TryParse(commentId, out Guid commentGuid))
                return BadRequest("Invalid comment ID format.");

            Comment? comment = await _db.GetCommentById(commentGuid);
            if (comment is null)
                return BadRequest("Unknown comment");

            long userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return this.HxUnauthorized();

            User? user = await _db.GetUserById(userId);
            if (user is null)
                return this.HxUnauthorized();

            await _db.UpdateLastLoginDateForUser(user);

            (bool isRatedAfter, bool? isUpAfter) = await _db.ChangeCommentRatingByUser(comment, user, isUp);

            return PartialView(viewName: "CommentRatingZone", model: new CommentRatingZoneViewModel(isRatedAfter, isUpAfter, commentId, comment.CountRating()));
        }

        [HttpPost]
        public async Task<IActionResult> RenderCommentReplyForm(string commentId)
        {
            if (!Guid.TryParse(commentId, out Guid commentGuid))
                return BadRequest("Invalid comment ID format.");

            long userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return this.HxUnauthorized();

            User? user = await _db.GetUserById(userId);
            if (user is null)
                return this.HxUnauthorized();

            throw new NotImplementedException();
        }
    }
}
