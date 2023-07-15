using Datings.Api.Common.Abstractions;

namespace Datings.Api.Common.Implementations.CodeValidator;

public class RedisCodeValidator : ICodeValidator
{
    public Task<bool> ValidateCode(string phone, string code)
    {
        throw new NotImplementedException();
    }

    public string GenerateCode(string phone)
    {
        throw new NotImplementedException();
    }
}