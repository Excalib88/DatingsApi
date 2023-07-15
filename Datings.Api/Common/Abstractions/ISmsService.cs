namespace Datings.Api.Common.Abstractions;

public interface ISmsService
{
    Task Send(string phone, string message);
    Task Call(string phone);
    Task<bool> VerifyCode(string phone, string code);
}