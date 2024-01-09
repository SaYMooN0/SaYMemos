using Microsoft.AspNetCore.Mvc;
using SaYMemos.Services.interfaces;
using ILogger = SaYMemos.Services.interfaces.ILogger;

namespace SaYMemos.Controllers
{
    public class ImagesController : Controller
    {
     

        IImageStorageService _imgStorage { get; init; }
        ILogger _logger { get; init; }
        public ImagesController(IImageStorageService imgStorage, ILogger logger)
        {
            _imgStorage = imgStorage;
            _logger = logger;
        }


        [HttpGet]
        [Route("images/{filePath}")]
        public IActionResult GetImage(string filePath)
        {
            try
            {
                var imageStream = _imgStorage.GetImage(filePath);
                return File(imageStream, "image/jpeg");
            }
            catch (Exception ex)
            {
                _logger.Error("Get Image method error", ex);
                return NotFound(ex.Message);
            }
        }
    }
}
