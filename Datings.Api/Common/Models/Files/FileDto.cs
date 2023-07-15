namespace Datings.Api.Common.Models.Files;

public class FileDto
{
    public string Bucket { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long ObjectSize { get; set; }
    public byte[] Content { get; set; } = null!;
}