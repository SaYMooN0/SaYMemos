using Microsoft.AspNetCore.Mvc;
using SaYMemos.Controllers.Helpers;
using SaYMemos.Models.form_classes;
using SaYMemos.Models.view_models;
using SaYMemos.Models.view_models.memos_page;
using SaYMemos.Services.interfaces;
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
            this.SetMemoFilter(MemoFilterForm.Default());
            this.SetMemoSortOptions(MemoSortOptionsForm.Default());
            return View(MemoPageViewModel.Default());
        }

        public IActionResult WithTagFilter(string tag)
        {
            var form = MemoFilterForm.DefaultWithTag(tag);
            this.SetMemoFilter(form);
            this.SetMemoSortOptions(MemoSortOptionsForm.Default());
            this.SetRenderedMemoCount(0);
            return View(viewName: "Index", new MemoPageViewModel(form, MemoSortOptionsForm.Default()));
        }


        [HttpPost]
        public async Task<IActionResult> SortingChanged(SortTypes sortType, bool isDescending)
        {
            MemoSortOptionsForm sorting=new(sortType, isDescending);
            this.SetMemoSortOptions(sorting);
            this.SetRenderedMemoCount(0);
            return await RenderNewPackage();
        }

        [HttpPost]
        public IActionResult RenderFoundTags(string? searchTag) =>
            string.IsNullOrWhiteSpace(searchTag) ?
            Content(string.Empty) :
            PartialView("FoundTags", _db.GetMatchingTags(searchTag));

        [HttpPost]
        public async Task<IActionResult> RenderNewPackage()
        {
            var sortOptions = this.GetMemoSortOptions();
            var filter = this.GetMemoFilter();
            int alreadyRendered=this.GetRenderedMemoCount();
            throw new NotImplementedException();
        }
        [HttpPost]
        public IActionResult AddFilterTag(string tag) =>
            PartialView("FilterTag", tag);
        [HttpPost]
        public IActionResult ClearFilter()
        {
            this.SetMemoFilter(MemoFilterForm.Default());
            this.SetRenderedMemoCount(0);
            //new filter view 
            //new memo package
            throw new NotImplementedException();
        }
        [HttpPost]
        public IActionResult ApplyFilter(MemoFilterForm filter)
        {
            this.SetMemoFilter(filter);
            this.SetRenderedMemoCount(0);
            throw new NotImplementedException();
            //set new memofiltercoockie
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
