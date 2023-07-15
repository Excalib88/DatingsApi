namespace Datings.Api.Common.Abstractions;

public interface ICodeValidator
{
    Task<bool> ValidateCode(string userName, string code);
    string GenerateCode(string userName);
}