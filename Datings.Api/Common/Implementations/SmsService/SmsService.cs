using Datings.Api.Common.Abstractions;

namespace Datings.Api.Common.Implementations.SmsService;

public class SmsService : ISmsService
{
    public Task Send(string phone, string message)
    {
        throw new NotImplementedException();
    }

    public Task Call(string phone)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyCode(string phone, string code)
    {
        throw new NotImplementedException();
    }
}