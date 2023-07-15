using Datings.Api.Common.Models.Profile;

namespace Datings.Api.BL;

public interface IProfileService
{
    Task UpdatePhoto(string data, byte[] buffer);
    Task UpdateFirstName(FioDto fio);
    Task UpdatePhoneNumber(string phone);
    Task<decimal> GetBalance();
    Task<List<BalanceHistoryModel>> GetBalanceHistory(BalanceHistoryFilters filters);
}