using Azure.Messaging.ServiceBus;
using FluentAssertions.Extensions;
using Testcontainers.ServiceBus;

namespace Hypothesist.ServiceBus.IntegrationTests;

public class Test : IAsyncLifetime
{
    private readonly ServiceBusContainer _bus = new ServiceBusBuilder()
        .WithAcceptLicenseAgreement(true)
        .Build();
    
    [Fact]
    public async Task Processor()
    {
        // Arrange
        await using var client = new ServiceBusClient(_bus.GetConnectionString());
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
        await using var client = new ServiceBusClient(_bus.GetConnectionString());
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

    Task IAsyncLifetime.InitializeAsync() => _bus.StartAsync();

    Task IAsyncLifetime.DisposeAsync() => _bus.DisposeAsync().AsTask();
}