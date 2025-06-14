using Amazon.S3;
using Amazon.S3.Model;
using FutFut.Profile.Service.Data;
using FutFut.Profile.Service.Enums;
using Microsoft.EntityFrameworkCore;

namespace FutFut.Profile.Service.HostedServices;

public class ObjectStorageCleanupHostedService(
        IServiceProvider serviceProvider,
        ILogger<ObjectStorageCleanupHostedService> logger,
        IAmazonS3 objectStorage
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using IServiceScope scope = serviceProvider.CreateScope();
                AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (await IsNeedToDoJob(dbContext))
                {
                    await DoCleanUpJob(dbContext, cancellationToken);
                    logger.LogInformation("Job:{job} is completed successfully.", "cleanup");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical("Something went wrong while starting job:{job}.", "cleanup");
            }

            await Task.Delay(TimeSpan.FromDays(1));
        }

    }

    public async Task<bool> IsNeedToDoJob(AppDbContext dbContext)
    {
        var lastWorkingTime =
            await dbContext.SystemWorks.FirstOrDefaultAsync(w => w.Name == SystemWorksEnum.ObjectStorageCleanUp.ToString());

        if (lastWorkingTime == null) return true;

        if ((DateTimeOffset.UtcNow - lastWorkingTime.TimeOfWork) - TimeSpan.FromDays(7) > TimeSpan.FromSeconds(1))
        {
            return true;
        }
        
        return false;
    }

    public async Task DoCleanUpJob(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        var objectsPathsInDb = new List<string>();
        objectsPathsInDb.AddRange(dbContext.Profiles.Select(p => p.Avatar).ToList());

        string? continuationToken = null;
        bool isObjectListTruncated;
        List<string> objectsPathsInObjectStorage = new List<string>();

        do
        {
            var response = await objectStorage.ListObjectsV2Async(new ListObjectsV2Request()
            {
                BucketName = "documents",
                ContinuationToken = continuationToken,
                Prefix = "profile"
            }, cancellationToken);
            
            continuationToken = response.NextContinuationToken;
            isObjectListTruncated = response.IsTruncated ?? false;

            if (response.S3Objects is { Count: > 0 })
            {
                objectsPathsInObjectStorage.AddRange(response.S3Objects.Select(o => o.Key).ToList());
            }
        }
        while(isObjectListTruncated);
        
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

            foreach (var objectPath in batch)
            {
                logger.LogInformation("Deleting {path} in object storage cause there is no references to this path in the database.", objectPath);
            }
            
            var deleteResponse = await objectStorage.DeleteObjectsAsync(new DeleteObjectsRequest()
            {
                BucketName = "documents",
                Objects = batch
            }, cancellationToken);
            
               
            foreach (var error in deleteResponse.DeleteErrors.ToList())
            {
                logger.LogError("Error while deleting object with key: {key}, message: {error}", error.Key, error.Message);
            }
        }
                
    }
}