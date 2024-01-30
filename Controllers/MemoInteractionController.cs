﻿using Microsoft.AspNetCore.Mvc;
using SaYMemos.Controllers.Helpers;
using SaYMemos.Models.data.entities.comments;
using SaYMemos.Models.data.entities.memos;
using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.view_models.memos;
using SaYMemos.Services.interfaces;
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

            await _db.UpdateLastLoginDateForUser(userId);

            bool isLikedAfter = await _db.ChangeLikeState(userId, parsedMemoId);
            return PartialView(viewName:"LikeIcon", new MemoLikeViewModel(isLikedAfter, memoId));
        }


        [HttpPost]
        public async Task<IActionResult> LeaveComment(string memoId, string memoComment)
        {
            
            if (!Guid.TryParse(memoId, out Guid parsedMemoId))
                return BadRequest("Invalid Memo ID format");
            if(string.IsNullOrEmpty(memoComment))
                return BadRequest("Invalid comment");

            long userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return this.HxUnauthorized();
            User? user = await _db.GetUserById(userId);
            if (user is null)
                return this.HxUnauthorized();

            await _db.UpdateLastLoginDateForUser(userId);

            Comment addedComment= await _db.AddCommentToMemo(parsedMemoId, memoComment, user);

            //saving comment
            return PartialView(viewName:"AddedComment");
        }
        [HttpPost]
        public async Task<IActionResult> RenderAllMemoInfo(string memoId)
        {
            if (!Guid.TryParse(memoId, out Guid parsedMemoId))
                return BadRequest("Invalid Memo ID format.");

            Memo? memo = await _db.GetMemoById(parsedMemoId);
            if (memo is null)
                return BadRequest("Unknown Memo ID format.");

            long userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return PartialView(viewName: "FullMemoInfo", model: MemoFullInfoViewModel.FromMemoForUnauthorized(memo));

            User? user = await _db.GetUserById(userId);
            if (user is null)
                return PartialView(viewName: "FullMemoInfo", model: MemoFullInfoViewModel.FromMemoForUnauthorized(memo));

            await _db.UpdateLastLoginDateForUser(userId);
            return PartialView(viewName: "FullMemoInfo", model: MemoFullInfoViewModel.FromMemoForUser(memo, user));
        }
    }
}
