using Datings.Api.BL;
using Datings.Api.Common.Implementations;
using Datings.Api.Common.Models;
using Datings.Api.Common.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Datings.Api.Controllers;

[Authorize]
[Route("profiles")]
public class ProfilesController : ApiController
{
    private readonly IProfileService _profileService;
    private readonly CurrentUser _currentUser;
    
    public ProfilesController(IProfileService profileService, UserProvider userProvider)
    {
        _profileService = profileService;
        _currentUser = userProvider.CurrentUser ?? throw new Exception();
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_currentUser);
    }

    [HttpPatch("photo")]
    public async Task<IActionResult> PatchPhoto(IFormFile formFile)
    {
        if (formFile.Length <= 0) return Ok();
        
        using var ms = new MemoryStream();
        await formFile.CopyToAsync(ms);
        await _profileService.UpdatePhoto(formFile.ContentType, ms.ToArray());

        return Ok();
    }

    [HttpPatch("fio")]
    public async Task<IActionResult> PatchFio(FioDto fioDto)
    {
        await _profileService.UpdateFirstName(fioDto);
        
        return Ok();
    }

    [HttpPatch("phone")]
    public async Task<IActionResult> PatchPhoneNumber(string phone)
    {
        await _profileService.UpdatePhoneNumber(phone);
        
        return Ok();
    }

    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        var balance = await _profileService.GetBalance();
        
        return Ok(new {balance});
    }

    [HttpGet("balance/history")]
    public async Task<IActionResult> GetBalanceHistory(BalanceHistoryFilters fiters)
    {
        var balanceHistory = await _profileService.GetBalanceHistory(fiters);
        
        return Ok(balanceHistory);
    }
}