namespace Datings.Api.Models.Identity;

public class SmsVerifyRequest
{
    public string Phone { get; set; } = null!;
    public string SmsCode { get; set; } = null!;
}