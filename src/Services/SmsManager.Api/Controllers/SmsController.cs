using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmsManager.Application.Request.Sms;
using SmsManager.Application.Response.Sms;
using SmsManager.Application.Services.Abstractions;

namespace SmsManager.Api.Controllers;

[ApiController]
[Route("sms")]
public class SmsController(ISmsService smsService, ILogger<SmsController> logger) : ControllerBase
{

    [Authorize(AuthenticationSchemes = "Basic")]
    [HttpGet("{id}")]
    public async Task<CreateSmsResponse> GetSmsInfoById([FromRoute] string id)
    {
        var identity = User.Identity as ClaimsIdentity;
        var currentUserId = identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        logger.LogInformation($"Current User Id:{currentUserId}");
        return await smsService.GetByIdAsync(id);
    }
    
    [HttpPost]
    public async Task<CreateSmsResponse> CreateSms([FromBody] CreateSmsRequest smsRequest)
    {
        return await smsService.AddAsync(smsRequest);
    }
}