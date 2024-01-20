namespace SaYMemos.Services.interfaces
{
    public interface IImageStorageService
    {
        public string SaveProfilePicture(Stream stream, long userId);
        public string SaveMemoImage(Stream stream, long authorId);
        public FileStream GetImage(string filePath);
        public string DefaultProfilePicture { get; }

        public Task<Tuple<string, Stream?>> TryConvertFormImageToStream(IFormFile picture);

    }
}
