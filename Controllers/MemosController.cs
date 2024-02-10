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

        public IActionResult Index()
        {
            MemoPreviewViewModel[] memos = Array.Empty<MemoPreviewViewModel>();//from db
            return View(model: MemoPageViewModel.Default(memos));
        }
        public IActionResult WithTagFilter(string tag)
        {
            MemoPreviewViewModel[] memos = Array.Empty<MemoPreviewViewModel>();//from db
            return View(viewName:"Index", model: new MemoPageViewModel(memos, MemoFilterForm.DefaultWithTag(tag), MemoSortOptionsForm.Default()));
        }

        [HttpPost]
        public IActionResult RenderFoundTags(string? searchTag) =>
            string.IsNullOrWhiteSpace(searchTag) ?
            Content(string.Empty) :
            PartialView("FoundTags", _db.GetMatchingTags(searchTag));


        [HttpPost]
        public IActionResult AddFilterTag(string tag) =>
            PartialView("FilterTag", tag);
        [HttpPost]
        public IActionResult ClearFilter()
        {
            //new filter view 
            //new memo package
            throw new NotImplementedException();
        }
        [HttpPost]
        public IActionResult ApplyFilter(MemoFilterForm filer)
        {
            throw new NotImplementedException();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
