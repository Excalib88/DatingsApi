using Datings.Api.Common.Models;

namespace Datings.Api.Common.Implementations;

public class UserProvider
{
    public CurrentUser? CurrentUser { get; set; }

    public void SetUser(CurrentUser user)
    {
        CurrentUser = user;
    }
}