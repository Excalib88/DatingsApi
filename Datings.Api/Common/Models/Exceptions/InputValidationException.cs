namespace Datings.Api.Common.Models.Exceptions;

public class InputValidationException : Exception
{
    public InputValidationException(string message = "") : base(message)
    {
    }
}