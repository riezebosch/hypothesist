using System.Threading.Tasks;
using FluentAssertions.Extensions;
using Rebus.Activation;
using Rebus.Config;
using Xunit;

namespace Hypothesist.Rebus.Tests;

public class AsHandlerTests : IClassFixture<RabbitMqContainer>
{
    private readonly RabbitMqContainer _container;

    public AsHandlerTests(RabbitMqContainer container) => 
        _container = container;

    [Fact]
    public async Task Test1()
    {
        var hypothesis = Hypothesis.For<UserLoggedIn>()
            .Any(x => x.Id == 1234);
        
        using var activator = new BuiltinHandlerActivator()
            .Register(hypothesis.AsHandler);
        activator.Handle<UserLoggedIn>(m => hypothesis.Test(m));
        var bus = Configure.With(activator)
            .Transport(t => t.UseRabbitMq(_container.ConnectionString, "consumer-queue"))
            .Start();

        await bus
            .Subscribe<UserLoggedIn>();

        var producer = Configure.With(new EmptyActivator())
            .Transport(t => t.UseRabbitMqAsOneWayClient(_container.ConnectionString))
            .Start();
        
        await producer
            .Publish(new UserLoggedIn(1234));

        await hypothesis
            .Validate(2.Seconds());
    }

    private record UserLoggedIn(int Id);
}

