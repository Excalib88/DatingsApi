using Microsoft.AspNetCore.Identity;

namespace Datings.Api.Data.Entities;

public class ApplicationUser : IdentityUser<long>
{
    public string? FirstName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? InterestsData { get; set; }
    public List<string> Interests => InterestsData?.Split('|').ToList() ?? new List<string>();
    public Gender? Gender { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    /// <summary>
    /// Photo in base64
    /// </summary>
    public List<UserPhoto> Photos { get; set; } = new();
    public string? FindNow { get; set; }
}

public enum Gender
{
    Male = 1,
    Female
}