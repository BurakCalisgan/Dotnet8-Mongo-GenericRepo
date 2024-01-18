namespace SmsManager.Application.Response.Auth;

public class AuthResponse(string id, string domainUsername, string password)
{
    public string Id { get; set; } = id;
    public string DomainUsername { get; set; } = domainUsername;
    public string Password { get; set; } = password;
}