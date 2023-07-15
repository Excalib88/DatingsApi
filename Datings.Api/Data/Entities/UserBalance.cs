namespace Datings.Api.Data.Entities;

public class UserBalance : BaseEntity
{
    public decimal Amount { get; set; }
    
    public ApplicationUser? User { get; set; }
    public long? UserId { get; set; }
}