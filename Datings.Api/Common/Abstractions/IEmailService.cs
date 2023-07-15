namespace Datings.Api.Common.Abstractions;

public interface IEmailService
{
    Task Send(string email, string title, string body);
}