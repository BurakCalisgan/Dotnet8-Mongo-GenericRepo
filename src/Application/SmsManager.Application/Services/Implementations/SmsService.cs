using SmsManager.Application.Request;
using SmsManager.Application.Response;
using SmsManager.Application.Services.Abstractions;
using SmsManager.Domain.Documents;
using SmsManager.Domain.Repository;

namespace SmsManager.Application.Services.Implementations;

public class SmsService(ISmsRepository smsRepository) : ISmsService
{
    public async Task<SmsResponse> AddAsync(CreateRequest request)
    {
        var result = await smsRepository.AddAsync(new Sms()
        {
            PhoneNumber = request.PhoneNumber,
            SmsContent = request.SmsContent,
            UpdatedAt = DateTime.UtcNow
        });

        return new SmsResponse
        {
            PhoneNumber = result.PhoneNumber,
            SmsContent = result.SmsContent,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt
        };
    }

    public async Task<SmsResponse> GetByIdAsync(string id)
    {
        var result = await smsRepository.GetByIdAsync(id);

        return new SmsResponse
        {
            PhoneNumber = result.PhoneNumber,
            SmsContent = result.SmsContent,
            ExternalSmsId = result.ExternalSmsId,
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt
        };
    }
}