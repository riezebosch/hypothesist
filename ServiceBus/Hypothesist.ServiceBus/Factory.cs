using Azure.Messaging.ServiceBus;

namespace Hypothesist.ServiceBus;

public static class Factory
{
    public static async Task<ServiceBusProcessor> For(
        this Observer<ServiceBusReceivedMessage> observer,
        ServiceBusProcessor processor,
        CancellationToken token = default)
    {
        processor.ProcessMessageAsync += e => observer.Add(e.Message, token);
        processor.ProcessErrorAsync += _ => Task.CompletedTask;

        await processor.StartProcessingAsync(token);
        return processor;
    }

    public static async Task<ServiceBusReceiver> For(
        this Observer<ServiceBusReceivedMessage> observer,
        ServiceBusReceiver receiver,    
        CancellationToken token = default)
    {
        var message = await receiver.ReceiveMessageAsync(cancellationToken: token);
        await observer.Add(message, token);

        return receiver;
    }
}