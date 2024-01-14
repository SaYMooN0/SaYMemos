namespace SaYMemos.Services.interfaces
{
    public interface IImageStorageService
    {
        public string SaveProfilePicture(Stream stream, string fileName);
        public FileStream GetImage(string filePath);
        public string DefaultProfilePicture { get; }
        public Task<MemoryStream> ConvertToJpgAsync(IFormFile picture);
    }
}
