namespace Datings.Api.Data.Entities;

public class UserPhoto: BaseEntity
{
    //base64
    public string? Data { get; set; }
    public long? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}