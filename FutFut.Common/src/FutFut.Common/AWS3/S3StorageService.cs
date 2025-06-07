using Amazon.S3;
using Amazon.S3.Model;
using FutFut.Common.Settings;
using Microsoft.Extensions.Configuration;

namespace FutFut.Common.AWS3;

public class S3StorageService(IAmazonS3 s3Client, IConfiguration configuration) : IS3StorageService
{
    private readonly string _bucket = configuration.GetSection(nameof(AWS3Settings)).Get<AWS3Settings>()!.Bucket;

    public async Task UploadAsync(Stream fileStream, string key, string contentType)
    {
        var request = new PutObjectRequest()
        {
            BucketName = _bucket,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType,
            CannedACL = S3CannedACL.PublicRead
        };
        
        await s3Client.PutObjectAsync(request);
    }
}