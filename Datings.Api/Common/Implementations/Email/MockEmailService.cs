using Datings.Api.Common.Abstractions;

namespace Datings.Api.Common.Implementations.Email;

public class MockEmailService : IEmailService
{
    private readonly ILogger<MockEmailService> _logger;

    public MockEmailService(ILogger<MockEmailService> logger)
    {
        _logger = logger;
    }

    public async Task Send(string email, string title, string body)
    {
        _logger.LogInformation($"Sent message. Email: {email}, Title: {title}, Body: {body}");

        await Task.FromResult(0);
    }
}