using System.Threading.Tasks;
using FluentAssertions.Extensions;
using MassTransit;
using NSubstitute;
using Xunit;

namespace Hypothesist.MassTransit.Tests;

public class AsConsumerTests
{
    [Fact]
    public async Task ConsumerTestsHypothesis()
    {
        // Arrange
        var observer = Observer.For<UserLoggedIn>();
        var consumer = observer
            .AsConsumer();

        var context = Substitute.For<ConsumeContext<UserLoggedIn>>();
        context.Message.Returns(new UserLoggedIn(3));

        // Act
        await consumer.Consume(context);
            
        // Assert
        await Hypothesis.On(observer)
            .Timebox(2.Seconds())
            .Any()
            .Match(new UserLoggedIn(3))
            .Validate();
    }

    public record UserLoggedIn(int Id);
}