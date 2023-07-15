using Datings.Api.Common.Implementations;
using Datings.Api.Common.Mappers;
using Datings.Api.Common.Models;
using Datings.Api.Common.Models.Exceptions;
using Datings.Api.Common.Models.Profile;
using Datings.Api.Data;
using Datings.Api.Data.Entities;
using Datings.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Datings.Api.BL;

public class ProfileService : IProfileService
{
    private readonly DataContext _context;
    private readonly CurrentUser _currentUser;

    public ProfileService(DataContext context, UserProvider userProvider)
    {
        _context = context;
        _currentUser = userProvider.CurrentUser ?? throw new Exception("Current user was null"); //todo: rework exceptions
    }

    public async Task UpdatePhoto(string data, byte[] buffer)
    {
        //todo: update photo
        var user = await _context.Users.FirstOrDefaultAsync(x => _currentUser.Id == x.Id);

        if (user == null) throw new EntityNotFoundException();

        var photoBase64 = Convert.ToBase64String(buffer);
        user.Photos.Add(new UserPhoto
        {
            Data = photoBase64,
            UserId = user.Id
        });
    }

    public async Task UpdateFirstName(FioDto fio)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => _currentUser.Id == x.Id);

        if (user == null) throw new EntityNotFoundException();

        if (!string.IsNullOrWhiteSpace(fio.FirstName))
        {
            user.FirstName = fio.FirstName;
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePhoneNumber(string phone)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => _currentUser.Id == x.Id);

        if (user == null) throw new EntityNotFoundException();

        if (!string.IsNullOrWhiteSpace(phone))
        {
            user.PhoneNumber = phone;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<decimal> GetBalance()
    {
        var operations = await _context.BalanceHistories.Where(x => x.UserId == _currentUser.Id).ToListAsync();
        decimal balance = 0;
        
        foreach (var operation in operations)
        {
            switch (operation.Type)
            {
                case BalanceOperationType.Debit or BalanceOperationType.Earning:
                    balance += operation.Amount;
                    break;
                case BalanceOperationType.Replenishment or BalanceOperationType.Spending:
                    balance -= operation.Amount;
                    break;
            }
        }
        
        return balance;
    }

    public async Task<List<BalanceHistoryModel>> GetBalanceHistory(BalanceHistoryFilters filters)
    {
        var query = _context.BalanceHistories.Where(x => x.UserId == _currentUser.Id);

        if (filters.DateFrom != null)
        {
            query = query.Where(x => x.CreatedAt < filters.DateFrom);
        }

        if (filters.DateTo != null)
        {
            query = query.Where(x => x.CreatedAt > filters.DateTo);
        }

        if (filters.Types != null && filters.Types.Any())
        {
            query = query.Where(x => filters.Types.Contains(x.Type));
        }
        
        return await query.Select(x => x.ToBalanceHistoryModel()).ToListAsync();
    }
}