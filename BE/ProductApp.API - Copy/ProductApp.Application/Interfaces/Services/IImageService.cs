namespace ProductApp.Application.Interfaces.Services;

public interface IImageService
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName, string folderName = "products");
    Task<bool> DeleteImageAsync(string imageUrl);
}
