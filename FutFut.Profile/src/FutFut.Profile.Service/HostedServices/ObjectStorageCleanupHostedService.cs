using Amazon.S3;
using Amazon.S3.Model;
using FutFut.Common.AWS3;
using FutFut.Profile.Service.Data;

namespace FutFut.Profile.Service.HostedServices;

public class ObjectStorageCleanupHostedService(IServiceProvider _serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        ILogger<ObjectStorageCleanupHostedService> logger = scope.ServiceProvider.GetRequiredService<ILogger<ObjectStorageCleanupHostedService>>();
        IAmazonS3 objectStorage = scope.ServiceProvider.GetRequiredService<IAmazonS3>();

        var objectsPathsInDb = new List<string>();
        objectsPathsInDb.AddRange(dbContext.Profiles.Select(p => p.Avatar).ToList());

        foreach (var path in objectsPathsInDb)
        {
            logger.LogInformation("Job:{job}. Deleting {path}", "cleanup", path);
        }

        string? continuationToken = null;
        bool isTruncated = false;
        List<string> objectsPathsInObjectStorage = new List<string>();

        do
        {
            var response = await objectStorage.ListObjectsV2Async(new ListObjectsV2Request()
            {
                BucketName = "documents",
                ContinuationToken = continuationToken
            }, cancellationToken);
            
            continuationToken = response.NextContinuationToken;
            isTruncated = response.IsTruncated ?? false;

            if (response.S3Objects != null && response.S3Objects .Count > 0)
            {
                objectsPathsInObjectStorage.AddRange(response.S3Objects.Select(o => o.Key).ToList());
            }
        }
        while(isTruncated);
        
        foreach (var path in objectsPathsInObjectStorage)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            logger.LogInformation("Job:{job}. Deleting {path}", "cleanup", path);
            Console.ResetColor();
        }
        
        var listOfObjectsToDelete = new List<KeyVersion>();
        
          
        foreach (var path in objectsPathsInObjectStorage)
        {
            if (!objectsPathsInDb.Contains(path))
            {
                listOfObjectsToDelete.Add(new KeyVersion() {Key = path});
            }
        }

        const int batchSize = 1000;

        for (int i = 0; i < listOfObjectsToDelete.Count; i += batchSize)
        {
            var batch = listOfObjectsToDelete.Skip(i).Take(batchSize).ToList();
            
            var deleteResponse = await objectStorage.DeleteObjectsAsync(new DeleteObjectsRequest()
            {
                BucketName = "documents",
                Objects = batch
            }, cancellationToken);
            
               
            foreach (var error in deleteResponse.DeleteErrors.ToList())
            {
              Console.ForegroundColor = ConsoleColor.Red;
              Console.WriteLine($"Error deleting {error.Message}");
              Console.ResetColor();
            }
        }
        
        
        logger.LogInformation("Job:{job} is completed successfully.", "cleanup");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}