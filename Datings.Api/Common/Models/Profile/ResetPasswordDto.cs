namespace Datings.Api.Common.Models.Profile;

public class ResetPasswordDto
{
    public string Email { get; set; } = null!;
    public string? Code { get; set; }
}