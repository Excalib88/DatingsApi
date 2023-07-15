namespace Datings.Api.Models.Identity;

public class AuthResponse
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}