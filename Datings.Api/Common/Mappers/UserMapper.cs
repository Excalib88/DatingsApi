using Datings.Api.Common.Models;
using Datings.Api.Data.Entities;

namespace Datings.Api.Common.Mappers;

public static class UserMapper
{
    public static UserModel? ToUserModel(this ApplicationUser? user)
    {
        return user == null
            ? null
            : new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName
            };
    }
}