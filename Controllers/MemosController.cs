using Microsoft.AspNetCore.Mvc;
using SaYMemos.Models.form_classes;
using SaYMemos.Models.view_models;
using SaYMemos.Models.view_models.memos;
using SaYMemos.Models.view_models.memos_page;
using SaYMemos.Services.interfaces;
using System.ComponentModel;
using System.Diagnostics;
using ILogger = SaYMemos.Services.interfaces.ILogger;

namespace SaYMemos.Controllers
{
    public class MemosController : Controller
    {
        IDatabase _db { get; init; }
        ILogger _logger { get; init; }
        IEncryptor _enc { get; init; }
        public MemosController(IDatabase db, ILogger logger, IEncryptor enc)
        {
            _db = db;
            _logger = logger;
            _enc = enc;
        }

        public IActionResult Index(MemoFilterForm? filter)
        {
            MemoPreviewViewModel[] memos = Array.Empty<MemoPreviewViewModel>(); //just for now
            if (filter is not null)
            {
                //memos from db with fileter
            }
            else
            {
                //first 10 memos
            }
            return View(model: MemoPageViewModel.Default(memos));
        }
        [HttpPost]
        public IActionResult RenderFoundTags(string? searchTag) =>
            string.IsNullOrWhiteSpace(searchTag) ?
            Content(string.Empty) :
            PartialView("FoundTags", _db.GetMatchingTags(searchTag));

        [HttpPost]
        public IActionResult ApplyFilter(MemoFilterForm filer)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public IActionResult ClearFilter()
        {
            //new filter view 
            //new memo package
            throw new NotImplementedException();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
