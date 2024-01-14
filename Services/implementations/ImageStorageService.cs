using SaYMemos.Services.interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
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


    public string SaveProfilePicture(Stream stream, string fileName)
    {
        EnsureProfilePicturesFolderCreated();
        return SaveImage(stream, fileName, ProfilePicturesFolder);
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
    private string SaveImage(Stream stream, string fileName, string? specifiedFolder = null)
    {
        string filePath;
        if (specifiedFolder is null)
            filePath = Path.Combine(ImageFolder, fileName);
        else
            filePath = Path.Combine(ImageFolder, specifiedFolder, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            stream.CopyTo(fileStream);
        }
        return filePath;
    }

    public async Task<MemoryStream> ConvertToJpgAsync(IFormFile picture)
    {
        using var inputStream = picture.OpenReadStream();
        using var image = await Image.LoadAsync(inputStream);
        var outputStream = new MemoryStream();
        var encoder = new JpegEncoder { Quality = 80 };
        await image.SaveAsJpegAsync(outputStream, encoder);
        outputStream.Seek(0, SeekOrigin.Begin);
        return outputStream;
    }

}
