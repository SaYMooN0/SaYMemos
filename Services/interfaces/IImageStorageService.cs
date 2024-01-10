namespace SaYMemos.Services.interfaces
{
    public interface IImageStorageService
    {
        string SaveProfilePicture(IFormFile file);
        public FileStream GetImage(string filePath);
        public string DefaultProfilePicture { get; }
    }
}
