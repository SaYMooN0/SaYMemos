using Microsoft.AspNetCore.Mvc;
using SaYMemos.Controllers.Helpers;
using SaYMemos.Models.data.entities.users;
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
            long userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return Unauthorized();

            User? user = await _db.GetUserByIdAsync(userId);
            if (user is null)
                return Unauthorized();

            await _db.UpdateLastLoginDateForUser(userId);


            if (!Guid.TryParse(memoId, out Guid parsedMemoId))
                return BadRequest("Invalid Memo ID format.");



            bool isLikedAfter = await _db.ChangeLikeState(userId, parsedMemoId);
            return PartialView(viewName: isLikedAfter ? "LikePressedIcon" : "LikeIcon", memoId);
        }


        [HttpPost]
        public IActionResult LeaveComment(string memoId, string memoComment)
        {
            
            if (!Guid.TryParse(memoId, out Guid parsedMemoId))
                return BadRequest("Invalid Memo ID format.");

            //saving comment
            return PartialView(viewName:"CommentSection");
        }
        [HttpPost]
        public IActionResult RenderAllMemoInfo(Guid memoId)
        {
            return PartialView(viewName: "FullMemoInfo");
        }
    }
}
