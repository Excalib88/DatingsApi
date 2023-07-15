using Datings.Api.Data.Models;

namespace Datings.Api.Data.Entities;

public class BalanceHistory : BaseEntity
{
    public decimal Amount { get; set; }
    public BalanceOperationType Type { get; set; }
    public ApplicationUser? User { get; set; }
    public long? UserId { get; set; }
}