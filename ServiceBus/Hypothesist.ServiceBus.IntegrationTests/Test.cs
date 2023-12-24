using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using FluentAssertions.Extensions;

namespace Hypothesist.ServiceBus.IntegrationTests;

public class Test : IAsyncLifetime
{
    private readonly string _queue = Guid.NewGuid().ToString("N");
    private const string ConnectionString = "sbmanuel.servicebus.windows.net";

    [Fact]
    public async Task Processor()
    {
        // Arrange
        await using var client = new ServiceBusClient(ConnectionString, new DefaultAzureCredential());
        await using var sender = client.CreateSender(_queue);
        await sender.SendMessageAsync(new ServiceBusMessage("data"));
        
        // Act
        var observer = Observer.For<ServiceBusReceivedMessage>();
        await using var processor = await observer.For(client.CreateProcessor(_queue));
        
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
        await using var client = new ServiceBusClient(ConnectionString, new DefaultAzureCredential());
        await using var sender = client.CreateSender(_queue);
        await sender.SendMessageAsync(new ServiceBusMessage("data"));
        
        // Act
        var observer = Observer.For<ServiceBusReceivedMessage>();
        await using var receiver = await observer.For(client.CreateReceiver(_queue));
        
        // Assert
        await Hypothesis
            .On(observer)
            .Timebox(30.Seconds())
            .Any()
            .Match(m => m.Body.ToString() == "data")
            .Validate();
    }

    async Task IAsyncLifetime.InitializeAsync()
    {
        var admin = new ServiceBusAdministrationClient(ConnectionString, new DefaultAzureCredential());
        await admin.CreateQueueAsync(_queue);
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        var admin = new ServiceBusAdministrationClient(ConnectionString, new DefaultAzureCredential());
        admin.DeleteQueueAsync(_queue); // don't wait for it to complete

        return Task.CompletedTask;
    }
}