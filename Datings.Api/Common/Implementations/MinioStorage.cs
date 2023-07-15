using Datings.Api.Common.Abstractions;
using Datings.Api.Common.Models.Files;
using Datings.Api.Common.Models.Options;
using Microsoft.Extensions.Options;
using Minio;

namespace Datings.Api.Common.Implementations;

public class MinioStorage : IFileStorage
{
    private readonly MinioOptions _minioOptions;

    public MinioStorage(IOptions<MinioOptions> minio)
    {
        _minioOptions = minio.Value;
    }

    public async Task<string> Upload(FileDto file)
    {
        using var minio = new MinioClient()
            .WithEndpoint(_minioOptions.Url)
            .WithCredentials(_minioOptions.Username, _minioOptions.Password)
            .WithSSL()
            .Build();

        if (!await minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(file.Bucket)))
        {
            await minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(file.Bucket));
        }

        using var ms = new MemoryStream(file.Content);
        
        var response = await minio.PutObjectAsync(new PutObjectArgs()
            .WithBucket(file.Bucket)
            .WithContentType(file.ContentType)
            .WithObject(Guid.NewGuid().ToString())
            .WithStreamData(ms)
            .WithObjectSize(file.ObjectSize));
        
        return response.ObjectName;
    }

    public async Task<GetFileResponse> Download(string bucketName, string objectName)
    {
        var buffer = Array.Empty<byte>();
        using var minio = new MinioClient()
            .WithEndpoint(_minioOptions.Url)
            .WithCredentials(_minioOptions.Username, _minioOptions.Password)
            .WithSSL()
            .Build();

        var existObject = await minio.StatObjectAsync(new StatObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName));

        if (existObject == null)
            throw new Exception();
        
        var ms = new MemoryStream();
        await minio.GetObjectAsync(new GetObjectArgs()
            .WithBucket("default")
            .WithObject(objectName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(ms);
                buffer = ms.ToArray();
                ms.Dispose();
            }));

        return new GetFileResponse
        {
            Buffer = buffer,
            Filename = existObject.ObjectName,
            ContentType = existObject.ContentType
        };
    }
}