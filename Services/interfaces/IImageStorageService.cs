namespace SaYMemos.Services.interfaces
{
    public interface IImageStorageService
    {
        public Task<string> SaveProfilePictureAsync(IFormFile file);
    }
}
