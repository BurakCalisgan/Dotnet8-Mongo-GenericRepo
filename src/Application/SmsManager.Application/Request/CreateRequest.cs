namespace SmsManager.Application.Request;

public class CreateRequest
{
    public string PhoneNumber { get; set; } = null!;
    public string SmsContent { get; set; } = null!;
}