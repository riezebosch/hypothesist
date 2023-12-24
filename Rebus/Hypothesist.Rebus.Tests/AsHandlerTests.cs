using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Rebus.Activation;
using Rebus.Config;
using Xunit;

namespace Hypothesist.Rebus.Tests;

public class AsHandlerTests(RabbitMqContainer container) : IClassFixture<RabbitMqContainer>
{
    [Fact]
    public async Task Test1()
    {
        var observer = Observer.For<UserLoggedIn>();
        using var activator = new BuiltinHandlerActivator()
            .Register(observer.AsHandler);

        var bus = Configure.With(activator)
            .Transport(t => t.UseRabbitMq(container.ConnectionString, "consumer-queue"))
            .Start();

        await bus
            .Subscribe<UserLoggedIn>();

        var producer = Configure.OneWayClient()
            .Transport(t => t.UseRabbitMqAsOneWayClient(container.ConnectionString))
            .Start();
        
        await producer
            .Publish(new UserLoggedIn(1234));

        await Hypothesis
            .On(observer)
            .Timebox(2.Seconds())
            .Any()
            .Match(new UserLoggedIn(1234))
            .Validate();
    }

    private record UserLoggedIn(int Id);
}

