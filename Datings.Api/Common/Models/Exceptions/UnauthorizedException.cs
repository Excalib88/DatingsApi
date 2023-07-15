namespace Datings.Api.Common.Models.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string? message = null) : base(message)
    {
    }
}