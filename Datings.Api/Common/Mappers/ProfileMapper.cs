using Datings.Api.Common.Models.Profile;
using Datings.Api.Data.Entities;

namespace Datings.Api.Common.Mappers;

public static class ProfileMapper
{
    public static BalanceHistoryModel ToBalanceHistoryModel(this BalanceHistory balanceHistory)
    {
        return  new BalanceHistoryModel
            {
                Amount = balanceHistory.Amount,
                Id = balanceHistory.Id,
                Type = balanceHistory.Type,
                CreatedAt = balanceHistory.CreatedAt
            };
    }
}