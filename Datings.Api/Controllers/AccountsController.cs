using System.IdentityModel.Tokens.Jwt;
using Datings.Api.BL;
using Datings.Api.Common.Models.Profile;
using Datings.Api.Data;
using Datings.Api.Data.Entities;
using Datings.Api.Extensions;
using Datings.Api.Identity;
using Datings.Api.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Datings.Api.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly IAccountsService _accountsService;

    public AccountsController(
        ITokenService tokenService, 
        DataContext context, 
        UserManager<ApplicationUser> userManager, 
        IConfiguration configuration, 
        IAccountsService accountsService)
    {
        _tokenService = tokenService;
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
        _accountsService = accountsService;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
    {
        var managedUser = await _userManager.FindByEmailAsync(request.Email);

        if (managedUser == null)
            return BadRequest(new {error = $"User {request.Email} not found"});
        
        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
        
        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }

        var roleIds = await _context.UserRoles
            .Where(r => r.UserId == managedUser.Id)
            .Select(x => x.RoleId)
            .ToListAsync();
        var roles = _context.Roles.Where(x => roleIds.Contains(x.Id)).ToList();
        
        var accessToken = _tokenService.CreateToken(managedUser, roles);
        managedUser.RefreshToken = _configuration.GenerateRefreshToken();
        managedUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());

        await _context.SaveChangesAsync();
        
        return Ok(new AuthResponse
        {
            Username = managedUser.UserName!,
            Email = managedUser.Email!,
            Phone = managedUser.PhoneNumber,
            Token = accessToken,
            RefreshToken = managedUser.RefreshToken
        });
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(request);
        
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            Email = request.Email, 
            UserName = request.Email,
            PhoneNumber = request.Phone
        };
        var result = await _userManager.CreateAsync(user, request.Password);

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        if (!result.Succeeded) return BadRequest(request);
        
        var findUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

        if (findUser == null) throw new Exception($"User {request.Email} not found");

        await _userManager.AddToRoleAsync(findUser, RoleConsts.Member);
            
        return await Login(new AuthRequest
        {
            Email = request.Email,
            Password = request.Password
        });
    }

    [HttpPost("password/reset")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        await _accountsService.ResetPassword(resetPasswordDto.Email);
        
        return Ok();
    }

    [HttpPost("password/verify/{code}")]
    public async Task<IActionResult> VerifyResetPassword(ResetPasswordDto resetPasswordDto)
    {
        if (string.IsNullOrWhiteSpace(resetPasswordDto.Code))
            return BadRequest(new {error = "Invalid code"});
        
        var result = await _accountsService.VerifyResetPassword(resetPasswordDto.Email, resetPasswordDto.Code);
        
        return Ok(new {verified = result});
    }

    [HttpPatch("password/change")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        if (changePasswordDto.Password != changePasswordDto.ConfirmPassword)
        {
            return BadRequest(new {error = "password is not equal confirm password"});
        }
        
        await _accountsService.ChangePassword(changePasswordDto.Email, changePasswordDto.Password);
        
        return Ok();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel? tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Invalid client request");
        }

        var accessToken = tokenModel.AccessToken;
        var refreshToken = tokenModel.RefreshToken;
        var principal = _configuration.GetPrincipalFromExpiredToken(accessToken);
        
        if (principal == null)
        {
            return BadRequest("Invalid access token or refresh token");
        }
        
        var username = principal.Identity!.Name;
        var user = await _userManager.FindByNameAsync(username!);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        var newAccessToken = _configuration.CreateToken(principal.Claims.ToList());
        var newRefreshToken = _configuration.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }
    
    [Authorize]
    [HttpPost("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return BadRequest("Invalid user name");

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return Ok();
    }
    
    [Authorize]
    [HttpPost("revoke-all")]
    public async Task<IActionResult> RevokeAll()
    {
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }

        return Ok();
    }
}