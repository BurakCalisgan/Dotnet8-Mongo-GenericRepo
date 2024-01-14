using System.Text;
using SmsManager.Domain.HttpClients;

namespace SmsManager.Infrastructure.HttpClients;

public class SmsClient : ISmsClient
{
    private readonly HttpClient _smsClient;

    public SmsClient(IHttpClientFactory httpClientFactory,HttpClient smsClient)
    {
        _smsClient = httpClientFactory.CreateClient("SmsApiClient");
    }

    public async Task<string> SendSms(string sms)
    {
        var requestContent = new StringContent(sms, Encoding.UTF8, "application/json");
        var response = await _smsClient.PostAsync("", requestContent);
        return await response.Content.ReadAsStringAsync();

    }
}