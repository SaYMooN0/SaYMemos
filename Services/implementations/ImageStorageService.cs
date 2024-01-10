using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;

public class ImageStorageService : IImageStorageService
{

    private readonly ILogger _logger;

    private const string
        ImageFolder= "images",
        ProfilePicturesFolder = "pp";
    private string DefaultImage=> Path.Combine(ImageFolder, "default.jpg");
    public string DefaultProfilePicture=> Path.Combine(ProfilePicturesFolder, "default.jpg");

    public ImageStorageService(ILogger logger)
    {
        _logger = logger;
        EnsureProfilePicturesFolderCreated();
    }
    

    public string SaveProfilePicture(IFormFile profilePicture)
    {
        EnsureProfilePicturesFolderCreated();
        return SaveImage(profilePicture, ProfilePicturesFolder);
    }
    

    public FileStream GetImage(string filePath)
    {
        var fullPath = Path.Combine(ImageFolder, filePath);
        if (File.Exists(fullPath))
            return new FileStream(fullPath, FileMode.Open, FileAccess.Read);

        return new FileStream(DefaultImage, FileMode.Open, FileAccess.Read);
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
    private string SaveImage(IFormFile file, string? specifiedFolder = null)
    {
        string filePath;
        if (specifiedFolder is null)
            filePath = Path.Combine(ImageFolder, file.FileName);
        else
            filePath = Path.Combine(ImageFolder, specifiedFolder, file.FileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(fileStream);
        }
        return filePath;
    }
}
