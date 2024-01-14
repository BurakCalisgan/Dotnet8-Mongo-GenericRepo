using SmsManager.Domain.Documents.Common;

namespace SmsManager.Domain.Documents;

public class Sms : Document
{
    public string PhoneNumber { get; set; } = null!;
    public string SmsContent { get; set; } = null!;
    public string? ExternalSmsId { get; set; }
}