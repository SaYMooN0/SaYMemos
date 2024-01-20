
namespace SaYMemos.Models.form_classes
{
    public record MemoCreationForm(
        string authorComment,
        IFormFile image,
        List<string> hashtags,
        string error
        )
    {
        public bool HasError()=>!string.IsNullOrEmpty( error );
        public bool HasImage()=>image is not null && image.Length>0;
        public MemoCreationForm Validate()
        {
            if (string.IsNullOrWhiteSpace(authorComment))
                return this with { error = "You cannot create memo without text" };
            string imageExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(imageExtension))
                return this with { error = $"Images with {imageExtension} extension are not supported. Please use .jpg, .jpeg or .png image" };
            return this with { error = string.Empty, hashtags = HandleHashtags() };
        }
        private List<string> HandleHashtags() =>
         hashtags?
             .Where(tag => !string.IsNullOrWhiteSpace(tag)) 
             .Distinct(StringComparer.OrdinalIgnoreCase) 
             .ToList() ?? new List<string>();

    }
}
