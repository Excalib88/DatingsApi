using Datings.Api.Common.Models.Files;

namespace Datings.Api.Common.Abstractions;

public interface IFileStorage
{
    Task<string> Upload(FileDto file);
    Task<GetFileResponse> Download(string bucket, string filename);
}