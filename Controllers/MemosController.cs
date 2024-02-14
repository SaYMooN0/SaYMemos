using Microsoft.AspNetCore.Mvc;
using SaYMemos.Controllers.Helpers;
using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.form_classes;
using SaYMemos.Models.view_models;
using SaYMemos.Models.view_models.memos;
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
            return View(viewName: "Index", new MemoPageViewModel(form, MemoSortOptionsForm.Default()));
        }


        [HttpPost]
        public async Task<IActionResult> SortingChanged(SortTypes sortType, bool isDescending)
        {
            MemoSortOptionsForm sorting = new(sortType, isDescending);
            this.SetMemoSortOptions(sorting);
            var filter = this.GetMemoFilter();
            return await RenderNewPackage(0, sorting, filter);
        }

        [HttpPost]
        public IActionResult RenderFoundTags(string? searchTag) =>
            string.IsNullOrWhiteSpace(searchTag) ?
            Content(string.Empty) :
            PartialView("FoundTags", _db.GetMatchingTags(searchTag));

        [HttpPost]
        public async Task<IActionResult> RenderNewPackage(int alreadyRendered)
        {
            var sortOptions = this.GetMemoSortOptions();
            var filter = this.GetMemoFilter();
            return await RenderNewPackage(alreadyRendered, sortOptions, filter);
        }
        private async Task<IActionResult> RenderNewPackage(int alreadyRendered, MemoSortOptionsForm sortOptions, MemoFilterForm filter)
        {
            var memos = await _db.GetMemoPackage(filter.GetFilterFunctions(), sortOptions, alreadyRendered);

            if (memos is null || memos.Count < 1)
                return MemosPackageView(new MemoPackageViewModel([], 0));
            long userId = this.GetUserId(_enc.DecryptId);
            if (userId == -1)
                return MemosPackageView(MemoPackageViewModel.FromMemosForUnauthorized(memos, alreadyRendered + 1));

            User? user = await _db.GetUserById(userId);
            if (user is null)
                return MemosPackageView(MemoPackageViewModel.FromMemosForUnauthorized(memos, alreadyRendered + 1));
            return View("MemosPackage", new MemoPackageViewModel(memos.Select(m => MemoPreviewViewModel.FromMemo(m, user)), alreadyRendered + 1));
        }
        public IActionResult MemosPackageView(MemoPackageViewModel model)
            => View(viewName: "MemosPackage", model);
        [HttpPost]
        public IActionResult AddFilterTag(string tag) =>
            PartialView("FilterTag", tag);
        [HttpPost]
        public async Task<IActionResult> ClearFilter()
        {
            this.SetMemoFilter(MemoFilterForm.Default());
            return View("EmptyFilter");
            //new memo package
            throw new NotImplementedException();
        }
        [HttpPost]
        public async Task<IActionResult> ApplyFilter(MemoFilterForm filter)
        {
            filter=filter is null? MemoFilterForm.Default(): filter;
            this.SetMemoFilter(filter);
            var sortOptions = this.GetMemoSortOptions();
            return await RenderNewPackage(0, sortOptions, filter);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
