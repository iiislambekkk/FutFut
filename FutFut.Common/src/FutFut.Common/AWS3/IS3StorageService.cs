namespace FutFut.Common.AWS3;

public interface IS3StorageService
{
    Task UploadAsync(Stream fileStream, string key, string contentType);
}