namespace SmsManager.Infrastructure.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string Database { get; set; } = null!;
}