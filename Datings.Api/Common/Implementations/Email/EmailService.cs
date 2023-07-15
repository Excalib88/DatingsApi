using Datings.Api.Common.Abstractions;
using FluentEmail.Core;

namespace Datings.Api.Common.Implementations.Email;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;

    public EmailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public async Task Send(string email, string title, string body)
    {
        await _fluentEmail
            .To(email)
            .Header(title, body)
            .SendAsync();
    }
}