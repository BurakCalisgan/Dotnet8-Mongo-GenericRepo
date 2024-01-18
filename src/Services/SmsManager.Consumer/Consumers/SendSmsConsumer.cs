using System.Text.Json;
using MassTransit;
using SmsManager.EventContract.Events;

namespace SmsManager.Consumer.Consumers;

public class SendSmsConsumer(ILogger<SendSmsConsumer> logger) : IConsumer<SendSmsEvent>
{
    public async Task Consume(ConsumeContext<SendSmsEvent> context)
    {
        
        var message = context.Message;
        logger.LogInformation($"{nameof(SendSmsConsumer)} message : {JsonSerializer.Serialize(message)}");
        await Task.CompletedTask;
    }
}

public class SendSmsConsumerDefinition : ConsumerDefinition<SendSmsConsumer>
{
    public SendSmsConsumerDefinition()
    {
        EndpointName = "Boyner.SmsManager_SendSmsConsumer";
    }
}