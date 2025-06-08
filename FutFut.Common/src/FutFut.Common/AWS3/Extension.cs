using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using FutFut.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FutFut.Common.AWS3;

public static class Extension
{
    public static IServiceCollection AddAWS3Storage(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>()!;
        var awsSettings = configuration.GetSection(nameof(AWS3Settings)).Get<AWS3Settings>()!;
        
        services.AddSingleton<IAmazonS3>(conf =>
            {
                var s3Config = new AmazonS3Config()
                {
                    RegionEndpoint = RegionEndpoint.USEast1,
                    ForcePathStyle = awsSettings.UseR2,
                    ServiceURL = awsSettings.UseR2 ? awsSettings.Endpoint : null,
                    RequestChecksumCalculation = RequestChecksumCalculation.WHEN_REQUIRED,
                    ResponseChecksumValidation = ResponseChecksumValidation.WHEN_REQUIRED
                };
                
                var credentials = new BasicAWSCredentials(awsSettings.AccessKey, awsSettings.SecretKey);

                return new AmazonS3Client(credentials,  s3Config);
            }
        );

        services.AddScoped<IS3StorageService, S3StorageService>();

        return services;
    }
}