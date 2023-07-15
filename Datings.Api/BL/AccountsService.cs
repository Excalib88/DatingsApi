using Datings.Api.Common.Abstractions;
using Datings.Api.Common.Extensions;
using Datings.Api.Common.Models.Exceptions;
using Datings.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace Datings.Api.BL;

public class AccountsService : IAccountsService
{
    private readonly IEmailService _emailService;
    private readonly IDistributedCache _cache;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountsService(IEmailService emailService, IDistributedCache cache, UserManager<ApplicationUser> userManager)
    {
        _emailService = emailService;
        _cache = cache;
        _userManager = userManager;
    }

    public async Task ResetPassword(string email)
    {
        var code = CodeGenerator.Generate();

        await _cache.SetStringAsync(email, code, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
        });
        await _emailService.Send(email, "Ваш код подтверждения от ExcalibQuestions", 
            $"<h2>Вы инициировали смену пароля в ExcalibQuestions.</h2><h2>Ваш код подтверждения: {code}</h2><h4>Ваш код действует в течении 2 часов</h4>");
    }

    public async Task<bool> VerifyResetPassword(string email, string code)
    {
        var cachedCode = await _cache.GetStringAsync(email);

        if (string.IsNullOrWhiteSpace(cachedCode) || cachedCode != code) 
            return false;
        
        await _cache.RemoveAsync(email);
        return true;
    }

    public async Task ChangePassword(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            throw new EntityNotFoundException();
        }
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _userManager.ChangePasswordAsync(user, token, password);
    }
}