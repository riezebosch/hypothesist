using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using FluentAssertions.Extensions;

namespace Hypothesist.ServiceBus.IntegrationTests;

public class ServiceBusReceivedMessageTests : IAsyncLifetime
{
    private readonly string _queue = Guid.NewGuid().ToString("N");
    private const string ConnectionString = "sbmanuel.servicebus.windows.net";

    [Fact]
    public async Task Test1()
    {
        // Arrange
        await using var client = new ServiceBusClient(ConnectionString, new DefaultAzureCredential());

        await using var sender = client.CreateSender(_queue);
        await sender.SendMessageAsync(new ServiceBusMessage("data"));
        
        // Act
        var hypothesis = Hypothesis
            .For<ServiceBusReceivedMessage>()
            .Any(m => m.Body.ToString() == "data");

        await using var processor = client.CreateProcessor(_queue);
        await processor.Test(hypothesis);
        
        // Assert
        await hypothesis.Validate(10.Seconds());
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