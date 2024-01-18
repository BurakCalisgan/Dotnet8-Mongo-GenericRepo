using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using SmsManager.Application.Services.Abstractions;

namespace SmsManager.Api.Security;

public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOption>
{
    private readonly IAuthService _authService;

    public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOption> options, ILoggerFactory logger,
        UrlEncoder encoder, IAuthService authService) : base(options, logger, encoder)
    {
        _authService = authService;
    }


    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return await Task.FromResult(AuthenticateResult.NoResult());
        }
        if (!AuthenticationHeaderValue.TryParse(Request.Headers.Authorization, out var authorization))
        {
            return await Task.FromResult(AuthenticateResult.NoResult());

        }

        if (!authorization.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
        {

            return await Task.FromResult(AuthenticateResult.NoResult());

        }
        
        byte[] credentialBytes = Convert.FromBase64String(authorization.Parameter ?? throw new InvalidOperationException());
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
        var domainName = credentials[0];
        var password = credentials[1];
        var result = await _authService
            .GetUserByDomainName(domainName,Convert.ToBase64String(Encoding.UTF8.GetBytes(password)));
        
        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.Name, domainName),
            new Claim(ClaimTypes.NameIdentifier, result.Id)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);


        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return await Task.FromResult(AuthenticateResult.Success(ticket));
        
        
    }
}