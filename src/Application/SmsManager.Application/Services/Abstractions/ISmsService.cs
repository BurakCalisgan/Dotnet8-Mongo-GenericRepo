using SmsManager.Application.Request;
using SmsManager.Application.Response;

namespace SmsManager.Application.Services.Abstractions;

public interface ISmsService
{
    Task<SmsResponse> AddAsync(CreateRequest request);
    Task<SmsResponse> GetByIdAsync(string id);
}