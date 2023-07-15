using Datings.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Datings.Api.Identity;

public interface ITokenService
{
    string CreateToken(ApplicationUser user, List<IdentityRole<long>> role);
}