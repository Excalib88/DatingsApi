using Datings.Api.Common.Abstractions;

namespace Datings.Api.Common.Implementations.SmsService;

public class MockSmsService : ISmsService
{
    private readonly ILogger<MockSmsService> _logger;

    public MockSmsService(ILogger<MockSmsService> logger)
    {
        _logger = logger;
    }

    public Task Send(string phone, string message)
    {
        _logger.LogInformation($"Send sms message {message} to {phone}");
        return Task.CompletedTask;
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