namespace SmsManager.Application.Request.Sms;

public class CreateSmsRequest
{
    public string PhoneNumber { get; set; } = null!;
    public string SmsContent { get; set; } = null!;
}