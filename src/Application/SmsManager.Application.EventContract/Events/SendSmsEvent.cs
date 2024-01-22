namespace SmsManager.Application.EventContract.Events;

public class SendSmsEvent
{
    public string PhoneNumber { get; set; } = null!;
    public string SmsContent { get; set; } = null!;
}