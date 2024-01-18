using System.Text;
using SmsManager.Application.Request.User;
using SmsManager.Application.Response.Auth;
using SmsManager.Application.Services.Abstractions;
using SmsManager.Domain.Documents;
using SmsManager.Domain.Repository;

namespace SmsManager.Application.Services.Implementations;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    public async Task AddAsync(CreateAuthUserRequest request)
    {
        await authRepository.AddAsync(new Auth
        {
            DomainName = request.DomainName,
            Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Password)),
            UpdatedAt = DateTime.UtcNow
        });
    }

    public async Task<AuthResponse> GetUserByDomainName(string domainName, string password)
    {
        var result = await authRepository
            .GetAsync(x => x.DomainName == domainName && x.Password == password);

        return new AuthResponse(result.Id, result.DomainName, result.Password);
    }
}