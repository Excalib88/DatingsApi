namespace Datings.Api.Common.Models.Files;

public class GetFileResponse
{
    public byte[] Buffer { get; set; } = null!;
    public string Filename { get; set; } = null!;
    public string ContentType { get; set; } = null!;
}