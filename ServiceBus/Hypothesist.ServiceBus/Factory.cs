using Azure.Messaging.ServiceBus;

namespace Hypothesist.ServiceBus;

public static class Factory
{
    public static async Task<ServiceBusProcessor> Test(
        this ServiceBusProcessor processor,
        IHypothesis<ServiceBusReceivedMessage> hypothesis,
        CancellationToken token = default)
    {
        processor.ProcessMessageAsync += e => hypothesis.Test(e.Message, token);
        processor.ProcessErrorAsync += _ => Task.CompletedTask;

        await processor.StartProcessingAsync(token);
        return processor;
    }

    public static async Task<ServiceBusReceiver> Test(
        this ServiceBusReceiver receiver,
        IHypothesis<ServiceBusReceivedMessage> hypothesis,
        CancellationToken token = default)
    {
        var message = await receiver.ReceiveMessageAsync(cancellationToken: token);
        await hypothesis.Test(message, token);

        return receiver;
    }
}