using Datings.Api.Data.Models;

namespace Datings.Api.Common.Models.Profile;

public class BalanceHistoryFilters
{
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public List<BalanceOperationType>? Types { get; set; }
}