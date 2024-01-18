namespace SmsManager.Application.Request.User;

public class CreateAuthUserRequest
{
    public string DomainName { get; set; } = null!;
    public string Password { get; set; } = null!;
}