using Datings.Api.Data.Models;

namespace Datings.Api.Common.Models.Profile;

public class BalanceHistoryModel
{
    public long Id { get; set; }
    public BalanceOperationType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}