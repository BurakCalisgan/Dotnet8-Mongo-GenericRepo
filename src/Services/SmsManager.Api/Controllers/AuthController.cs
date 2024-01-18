using Microsoft.AspNetCore.Mvc;
using SmsManager.Application.Request.User;
using SmsManager.Application.Services.Abstractions;

namespace SmsManager.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost]
    public async Task CreateAuthUser([FromBody] CreateAuthUserRequest request)
    {
        await auth.AddAsync(request);
    }
}