using SmsManager.Application.Request.User;
using SmsManager.Application.Response.Auth;

namespace SmsManager.Application.Services.Abstractions;

public interface IAuthService
{
    Task AddAsync(CreateAuthUserRequest request);
    Task<AuthResponse> GetUserByDomainName(string domainName, string password);
}