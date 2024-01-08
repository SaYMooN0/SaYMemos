using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;

public class ImageStorageService : IImageStorageService
{

    private readonly ILogger _logger;

    private const string
        ImageFolder= "images",
        ProfilePicturesFolder = "pp";

    public ImageStorageService(ILogger logger)
    {
        _logger = logger;
        EnsureProfilePicturesFolderCreated();
    }
    

    public async Task<string> SaveProfilePictureAsync(IFormFile profilePicture)
    {
        EnsureProfilePicturesFolderCreated();
        return await SaveImageAsync(profilePicture, ProfilePicturesFolder);
    }
    private void EnsureProfilePicturesFolderCreated() =>
        EnsureFolderCreated(Path.Combine(ImageFolder, ProfilePicturesFolder));


    private void EnsureFolderCreated(string folderPath)
    {
        try
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }
        catch (Exception ex)
        {
            _logger.Error($"Error creating folder '{folderPath}': {ex.Message}");
            throw;
        }
    }
    private async Task<string> SaveImageAsync(IFormFile file, string? specifiedFolder = null)
    {
        string filePath;
        if (specifiedFolder is null)
            filePath = Path.Combine(ImageFolder, file.FileName);
        else
            filePath = Path.Combine(ImageFolder, specifiedFolder, file.FileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return filePath;
    }
}
