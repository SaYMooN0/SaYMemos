
namespace SaYMemos.Models.form_classes
{
    public record MemoCreationForm(
        string authorComment,
        IFormFile? image,
        List<string> hashtags,
        bool areCommentsAvailable,
        string error
        )
    {
        public bool HasError() => !string.IsNullOrEmpty(error);
        public bool HasImage() => image is not null && image.Length > 0;
        public MemoCreationForm Validate()
        {
            if (string.IsNullOrWhiteSpace(authorComment))
                return this with { error = "You cannot create memo without text" };
            if (authorComment.Length > 1023)
                return this with { error = "The text is too long. Maximum number of characters is 1023. Characters count: " + authorComment.Length };

            if (image is not null)
            {
                string imageExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(imageExtension))
                    return this with { error = $"Images with {imageExtension} extension are not supported. Please use .jpg, .jpeg or .png image" };
            }
            return this with { error = string.Empty, hashtags = HandleHashtags() };
        }
        private List<string> HandleHashtags() =>
         hashtags?
             .Where(tag => !string.IsNullOrWhiteSpace(tag))
             .Distinct(StringComparer.OrdinalIgnoreCase)
             .ToList() ?? new List<string>();

    }
}
