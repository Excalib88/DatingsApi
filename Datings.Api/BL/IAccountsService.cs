namespace Datings.Api.BL;

public interface IAccountsService
{
    Task ResetPassword(string email);
    Task<bool> VerifyResetPassword(string email, string code);
    Task ChangePassword(string email, string password);
}