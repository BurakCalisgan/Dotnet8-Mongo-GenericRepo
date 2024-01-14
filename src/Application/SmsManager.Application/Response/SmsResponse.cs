namespace SmsManager.Application.Response;

public class SmsResponse
{
    public string PhoneNumber { get; set; } = null!;
    public string SmsContent { get; set; } = null!;
    public string? ExternalSmsId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}