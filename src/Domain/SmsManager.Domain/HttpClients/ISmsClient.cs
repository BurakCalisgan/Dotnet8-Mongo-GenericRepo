namespace SmsManager.Domain.HttpClients;

public interface ISmsClient
{
    Task<string> SendSms(string sms);
}