using Azure.Messaging.ServiceBus;
using FluentAssertions.Extensions;

namespace Hypothesist.ServiceBus.IntegrationTests;

public class Test : IClassFixture<Fixture>
{
    [Fact]
    public async Task Processor()
    {
        // Arrange
        await using var client = new ServiceBusClient("Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;");
        await using var sender = client.CreateSender("queue.1");
        await sender.SendMessageAsync(new ServiceBusMessage("data"));
        
        // Act
        var observer = Observer.For<ServiceBusReceivedMessage>();
        await using var processor = await observer.For(client.CreateProcessor("queue.1"));
        
        // Assert
        await Hypothesis
            .On(observer)
            .Timebox(10.Seconds())
            .Any()
            .Match(m => m.Body.ToString() == "data")
            .Validate();
    }
    
    [Fact]
    public async Task Receiver()
    {
        // Arrange
        await using var client = new ServiceBusClient("Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;");
        await using var sender = client.CreateSender("queue.1");
        await sender.SendMessageAsync(new ServiceBusMessage("data"));
        
        // Act
        var observer = Observer.For<ServiceBusReceivedMessage>();
        await using var receiver = await observer.For(client.CreateReceiver("queue.1"));
        
        // Assert
        await Hypothesis
            .On(observer)
            .Timebox(30.Seconds())
            .Any()
            .Match(m => m.Body.ToString() == "data")
            .Validate();
    }
}