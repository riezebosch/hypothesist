using Azure.Messaging.ServiceBus;

namespace Hypothesist.ServiceBus;

public static class Factory
{
    public static async Task Test(this ServiceBusProcessor processor, IHypothesis<ServiceBusReceivedMessage> hypothesis)
    {
        processor.ProcessMessageAsync += e => hypothesis.Test(e.Message);
        processor.ProcessErrorAsync += _ => Task.CompletedTask;

        await processor.StartProcessingAsync();
    }
}