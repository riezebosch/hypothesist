using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using FluentAssertions.Extensions;
using Testcontainers.MsSql;

namespace Hypothesist.ServiceBus.IntegrationTests;

public class Fixture : IAsyncLifetime
{
    private readonly INetwork _network;
    private readonly IContainer _container;
    private readonly IContainer _sql;

    public Fixture()
    {
        _network = new NetworkBuilder()
            .WithName("hypothesist")
            .Build();

        var password = Guid.NewGuid().ToString();
        _container = new ContainerBuilder()
            .WithImage("mcr.microsoft.com/azure-messaging/servicebus-emulator")
            .WithPortBinding(5672)
            .WithResourceMapping(new FileInfo("config.json"), "/ServiceBus_Emulator/ConfigFiles/")
            .WithEnvironment("SQL_SERVER", "sqlserver")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("MSSQL_SA_PASSWORD", password)
            .WithNetwork(_network)
            .WithWaitStrategy(
                Wait.ForUnixContainer().UntilMessageIsLogged("Emulator Service is Successfully Up!",o => o.WithTimeout(TimeSpan.FromMinutes(2))))
            .Build();
        
        _sql = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:latest")
            .WithName("sqlserver")
            .WithPassword(password)
            .WithNetwork(_network)
            .Build();
    }

    async Task IAsyncLifetime.InitializeAsync()
    {
        await _sql.StartAsync();
        await _container.StartAsync();
        await Task.Delay(1.Seconds()); // emulator needs a little extra time
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _container.DisposeAsync();
        await _sql.DisposeAsync();
        await _network.DisposeAsync();
    }
}