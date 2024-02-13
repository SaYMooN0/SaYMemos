using SaYMemos.Models.data.entities.memos;

namespace SaYMemos.Models.form_classes
{
    public record class MemoFilterForm(
        bool onlyWithImages,
        string[] tags,
        DateOnly? dateFrom,
        DateOnly? dateTo,
        int? likesFrom,
        int? likesTo,
        int? commentsFrom,
        int? commentsTo)
    {
        public static MemoFilterForm Default() => new(
                false, Array.Empty<string>(),
                null, null, null, null, null, null);
        public static MemoFilterForm DefaultWithTag(string tag) => new(
               false, [tag],
               null, null, null, null, null, null);
        public List<MemoFilterFunc> GetFilterFunctions()
        {
            var filters = new List<MemoFilterFunc>();

            if (onlyWithImages)
                filters.Add(query => query.Where(m => !string.IsNullOrEmpty(m.imagePath)));

            if (tags?.Length > 0)
                filters.Add(query => query.Where(m => m.Tags.Any(tag => tags.Contains(tag.Value))));

            if (dateFrom.HasValue)
            {
                var fromDateTime = dateFrom.Value.ToDateTime(TimeOnly.MinValue);
                filters.Add(query => query.Where(m => m.creationTime >= fromDateTime));
            }

            if (dateTo.HasValue)
            {
                var toDateTime = dateTo.Value.ToDateTime(TimeOnly.MaxValue);
                filters.Add(query => query.Where(m => m.creationTime <= toDateTime));
            }

            if (likesFrom.HasValue)
                filters.Add(query => query.Where(m => m.Likes.Count >= likesFrom.Value));

            if (likesTo.HasValue)
                filters.Add(query => query.Where(m => m.Likes.Count <= likesTo.Value));

            if (commentsFrom.HasValue)
                filters.Add(query => query.Where(m => m.Comments.Count >= commentsFrom.Value));

            if (commentsTo.HasValue)
                filters.Add(query => query.Where(m => m.Comments.Count <= commentsTo.Value));

            return filters;
        }
    }
}
