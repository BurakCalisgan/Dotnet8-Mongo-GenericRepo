using Microsoft.AspNetCore.Mvc;
using SmsManager.Application.Request;
using SmsManager.Application.Response;
using SmsManager.Application.Services.Abstractions;

namespace SmsManager.Api.Controllers;

[ApiController]
[Route("sms")]
public class SmsController(ISmsService smsService) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<SmsResponse> GetSmsInfoById([FromRoute] string id)
    {
        return await smsService.GetByIdAsync(id);
    }
    
    [HttpPost]
    public async Task<SmsResponse> CreateSms([FromBody] CreateRequest request)
    {
        return await smsService.AddAsync(request);
    }
}