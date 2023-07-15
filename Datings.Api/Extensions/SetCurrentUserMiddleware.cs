using System.Security.Claims;
using Datings.Api.Common.Implementations;
using Datings.Api.Common.Models;
using Datings.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Datings.Api.Extensions;

public class SetCurrentUserMiddleware
{
    private readonly RequestDelegate _next;

    public SetCurrentUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, UserProvider userProvider, DataContext context)
    {
        if (httpContext.User.Identity?.IsAuthenticated != true)
        {
            await _next(httpContext);
            return;
        }
        
        var userEmail = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrWhiteSpace(userEmail))
        {
            await _next(httpContext);
            return;
        }
        
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Email == userEmail);

        if (user == null)
        {
            await _next(httpContext);
            return;
        }
        
        userProvider.SetUser(new CurrentUser
        {
            Id = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            FirstName = user.FirstName,
            PhoneNumber = user.PhoneNumber
        });

        await _next(httpContext);
    }
}