using SmsManager.Domain.Documents.Common;

namespace SmsManager.Domain.Documents;

public class Auth : Document
{
    public string DomainName { get; set; } = null!;
    public string Password { get; set; } = null!;
}