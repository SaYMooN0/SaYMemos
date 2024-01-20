using SaYMemos.Services.interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using ILogger = SaYMemos.Services.interfaces.ILogger;

public class ImageStorageService : IImageStorageService
{

    private readonly ILogger _logger;

    private const string
        ImageFolder= "images",
        MemoImagesFolder= "memos",
        ProfilePicturesFolder = "pp";
    private string DefaultImage=> Path.Combine(ImageFolder, "default.jpg");
    public string DefaultProfilePicture=> Path.Combine(ProfilePicturesFolder, "default.jpg");

    public ImageStorageService(ILogger logger)
    {
        _logger = logger;
    }


    public string SaveProfilePicture(Stream stream, long userId)=>
        SaveImage(stream, userId.ToString()+".jpg", ProfilePicturesFolder);


    public FileStream GetImage(string filePath)
    {
        var fullPath = Path.Combine(ImageFolder, filePath);
        if (File.Exists(fullPath))
            return new FileStream(fullPath, FileMode.Open, FileAccess.Read);

        return new FileStream(DefaultImage, FileMode.Open, FileAccess.Read);
    }

    public string SaveMemoImage(Stream stream, long authorId)=>
        SaveImage(stream, Guid.NewGuid().ToString()+".jpg", Path.Combine(MemoImagesFolder,authorId.ToString()));

    public async Task<Tuple<string, Stream?>> TryConvertFormImageToStream(IFormFile picture)
    {

        string imageExtension = Path.GetExtension(picture.FileName).ToLowerInvariant();
        if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(imageExtension))
            return new($"Images with {imageExtension} extension are not supported. Use .jpg, .jpeg or .png image", null);
        if (imageExtension == ".png")
            return new(string.Empty, await ConvertToJpg(picture));
        else
            return new(string.Empty, picture.OpenReadStream());
    }


    private async Task<MemoryStream> ConvertToJpg(IFormFile picture)
    {
        using var inputStream = picture.OpenReadStream();
        using var image = await Image.LoadAsync(inputStream);
        var outputStream = new MemoryStream();
        var encoder = new JpegEncoder { Quality = 80 };
        await image.SaveAsJpegAsync(outputStream, encoder);
        outputStream.Seek(0, SeekOrigin.Begin);
        return outputStream;
    }
    private string SaveImage(Stream stream, string fileName, string? specifiedFolder = null)
    {
        string filePath;
        if (specifiedFolder is null)
        {
            filePath = Path.Combine(ImageFolder, fileName);
            EnsureFolderCreated(ImageFolder);
        }
        else
        {
            filePath = Path.Combine(ImageFolder, specifiedFolder, fileName);
            EnsureFolderCreated(Path.Combine(ImageFolder, specifiedFolder));
        }

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            stream.CopyTo(fileStream);
        }
        return filePath;
    }
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
}
