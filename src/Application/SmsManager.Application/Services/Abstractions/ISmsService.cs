using SmsManager.Application.Request.Sms;
using SmsManager.Application.Response.Sms;

namespace SmsManager.Application.Services.Abstractions;

public interface ISmsService
{
    Task<CreateSmsResponse> AddAsync(CreateSmsRequest smsRequest);
    Task<CreateSmsResponse> GetByIdAsync(string id);
}