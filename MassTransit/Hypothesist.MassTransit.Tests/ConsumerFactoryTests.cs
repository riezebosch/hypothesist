using System.Threading.Tasks;
using FluentAssertions.Extensions;
using MassTransit;
using NSubstitute;
using Xunit;

namespace Hypothesist.MassTransit.Tests
{
    public class ConsumerFactoryTests
    {
        [Fact]
        public async Task ConsumerTestsHypothesis()
        {
            // Arrange
            var hypothesis = Hypothesis.For<UserLoggedIn>()
                .Any(x => x == new UserLoggedIn(3));
            var consumer = hypothesis
                .AsConsumer();

            var context = Substitute.For<ConsumeContext<UserLoggedIn>>();
            context.Message.Returns(new UserLoggedIn(3));

            // Act
            await consumer.Consume(context);
            
            // Assert
            await hypothesis.Validate(2.Seconds());
        }

        public record UserLoggedIn(int Id);
    }
}