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
        public static MemoFilterForm Default() =>
            new(
                false, Array.Empty<string>(),
                null, null, null, null, null, null);
    }
}
