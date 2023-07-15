using Datings.Api.Common.Abstractions;
using Datings.Api.Common.Extensions;

namespace Datings.Api.Common.Implementations.CodeValidator;

public class MemoryCodeValidator : ICodeValidator
{
    private readonly Dictionary<string, string> _userCodes = new();

    public Task<bool> ValidateCode(string userName, string code)
    {
        return _userCodes.TryGetValue(userName, out var smsCode) 
            ? Task.FromResult(smsCode == code) 
            : Task.FromResult(false);
    }

    public string GenerateCode(string userName)
    {
        var code = CodeGenerator.Generate();

        if (_userCodes.ContainsKey(userName))
        {
            _userCodes[userName] = code;
        }
        else
        {
            _userCodes.Add(userName, code);
        }
        
        return code;
    }
}