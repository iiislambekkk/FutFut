using System.Net;
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
            DisablePayloadSigning = true, 
        };
        
        var response = await s3Client.PutObjectAsync(request);
        Console.WriteLine(response.ETag);
    }

    public async Task<bool> DeleteAsync(string key)
    {
        var request = new DeleteObjectRequest()
        {
            BucketName = _bucket,
            Key = key
        };

        var response = await s3Client.DeleteObjectAsync(request);

        return response.HttpStatusCode == HttpStatusCode.NoContent;
    }
}