using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductApp.Application.Interfaces.Services;

namespace ProductApp.Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly IHostEnvironment _environment;
    private readonly ILogger<ImageService> _logger;

    public ImageService(IHostEnvironment environment, ILogger<ImageService> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string folderName = "products")
    {
        if (imageStream == null || imageStream.Length == 0)
            throw new ArgumentException("File ảnh không hợp lệ");

        // Validate file extension
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
            throw new ArgumentException("Chỉ chấp nhận file ảnh: .jpg, .jpeg, .png, .gif, .webp");

        // Create folder if not exists
        var contentRootPath = _environment.ContentRootPath;
        var uploadsFolder = Path.Combine(contentRootPath, "wwwroot", "uploads", folderName);
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        // Generate unique filename
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Save file
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageStream.CopyToAsync(fileStream);
        }

        // Return relative URL
        var imageUrl = $"/uploads/{folderName}/{uniqueFileName}";
        _logger.LogInformation($"Image uploaded successfully: {imageUrl}");
        
        return imageUrl;
    }

    public async Task<bool> DeleteImageAsync(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl))
            return false;

        try
        {
            var fileName = Path.GetFileName(imageUrl);
            var folderPath = Path.GetDirectoryName(imageUrl)?.Replace("/", "\\").TrimStart('\\');
            
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderPath))
                return false;

            var contentRootPath = _environment.ContentRootPath;
            var fullPath = Path.Combine(contentRootPath, "wwwroot", folderPath, fileName);
            
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation($"Image deleted successfully: {imageUrl}");
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting image: {imageUrl}");
            return false;
        }
    }
}
