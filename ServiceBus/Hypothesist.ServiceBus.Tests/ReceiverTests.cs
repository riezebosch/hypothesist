using System.Diagnostics;
using Azure.Messaging.ServiceBus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NSubstitute;

namespace Hypothesist.ServiceBus.Tests;

public class ReceiverTests
{
    [Fact]
    public async Task Cancelled()
    {
        // Arrange
        var hypothesis = Hypothesis
            .For<ServiceBusReceivedMessage>()
            .Any();
        
        var receiver = Substitute.For<ServiceBusReceiver>();
        receiver
            .ReceiveMessageAsync(Arg.Any<TimeSpan?>(), Arg.Any<CancellationToken>())
            .Returns(async info =>
            {
                await Task.Delay(1.Minutes(), info.Arg<CancellationToken>());
                return null;
            });

        var sw = Stopwatch.StartNew();
        var token = new CancellationTokenSource(10);
        
        // Act
        var act = () => receiver.Test(hypothesis, token: token.Token);
        
        // Assert
        await act.Should().ThrowAsync<TaskCanceledException>();
        sw.Elapsed.Should().BeLessThan(1.Seconds());
    }
}