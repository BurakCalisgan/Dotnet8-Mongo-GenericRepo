using MassTransit;
using SmsManager.Application.Request.Sms;
using SmsManager.Application.Response.Sms;
using SmsManager.Application.Services.Abstractions;
using SmsManager.Domain.Documents;
using SmsManager.Domain.Repository;
using SmsManager.EventContract.Events;

namespace SmsManager.Application.Services.Implementations;

public class SmsService(ISmsRepository smsRepository, IPublishEndpoint publishEndpoint) : ISmsService
{
    public async Task<CreateSmsResponse> AddAsync(CreateSmsRequest smsRequest)
    {
        var result = await smsRepository.AddAsync(new Sms()
        {
            PhoneNumber = smsRequest.PhoneNumber,
            SmsContent = smsRequest.SmsContent,
            UpdatedAt = DateTime.UtcNow
        });
        await publishEndpoint.Publish(new SendSmsEvent
        {
            PhoneNumber = smsRequest.PhoneNumber,
            SmsContent = smsRequest.SmsContent,
        });

        return new CreateSmsResponse
        {
            PhoneNumber = result.PhoneNumber,
            SmsContent = result.SmsContent,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt
        };
    }

    public async Task<CreateSmsResponse> GetByIdAsync(string id)
    {
        var result = await smsRepository.GetByIdAsync(id);

        return new CreateSmsResponse
        {
            PhoneNumber = result.PhoneNumber,
            SmsContent = result.SmsContent,
            ExternalSmsId = result.ExternalSmsId,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt
        };
    }
}