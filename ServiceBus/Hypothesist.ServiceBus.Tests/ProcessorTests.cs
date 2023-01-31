using Azure.Messaging.ServiceBus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NSubstitute;

namespace Hypothesist.ServiceBus.Tests;

public class ProcessorTests
{
    [Fact]
    public async Task Cancelled()
    {
        // Arrange
        var hypothesis = Hypothesis
            .For<ServiceBusReceivedMessage>()
            .Any();
        
        var receiver = Substitute.For<ServiceBusProcessor>();
        receiver
            .StartProcessingAsync(Arg.Any<CancellationToken>())
            .Returns(info => Task.Delay(10.Seconds(), info.Arg<CancellationToken>()));

        var token = new CancellationTokenSource(10);
        
        // Act
        var act = () => receiver.Test(hypothesis, token.Token);
        
        // Assert
        await act.Should().ThrowAsync<TaskCanceledException>();
    }
}